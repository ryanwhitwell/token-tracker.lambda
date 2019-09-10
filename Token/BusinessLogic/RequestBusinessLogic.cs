using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.InSkillPricing;
using Alexa.NET.InSkillPricing.Responses;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Token.BusinessLogic.RequestHandlers;
using Token.Core;
using Token.DataAccess.Interfaces;
using Token.Models;

namespace Token.BusinessLogic
{
    public class RequestBusinessLogic: IRequestBusinessLogic
    {
        private static readonly string POINTS_PERSISTENCE_PRODUCT_ID = Configuration.File.GetSection("InSkillProducts")["PointsPersistence"];
        private ITokenUserData tokenUserData;
        private ILogger<RequestBusinessLogic> logger;
        private IEnumerable<IRequestHandler> requestHandlers;
        public RequestBusinessLogic (ILogger<RequestBusinessLogic> logger, IEnumerable<IRequestHandler> requestHandlers, ITokenUserData tokenUserData)
        {
            if (logger is null)
            {
                throw new ArgumentNullException("logger");
            }

            if (requestHandlers is null || requestHandlers.Count() <= 0)
            {
                throw new ArgumentNullException("requestHandlers");
            }

            if (tokenUserData is null)
            {
                throw new ArgumentNullException("tokenUserData");
            }

            this.logger = logger;
            this.requestHandlers = requestHandlers;
            this.tokenUserData = tokenUserData;
        }

        private static TokenUser GenerateEmptyTokenUser(string id)
        {
            TokenUser tokenUser = new TokenUser()
                { 
                    Id = id, 
                    Players = new List<Player>()
                };

                return tokenUser;
        }

        public async Task<bool> HasPointsPersistence(SkillRequest skillRequest)
        {
            this.logger.LogTrace("BEGIN HasPointsPersistence. RequestId: {0}, UserId: {1}.", skillRequest.Request.RequestId, skillRequest.Context.System.User.UserId);

            if (skillRequest == null)
            {
                throw new ArgumentNullException("skillRequest");
            }

            if (skillRequest.Context.System.ApiEndpoint == null)
            {
                throw new ArgumentNullException("apiEndpoint");
            }

            if (skillRequest.Context.System.ApiAccessToken == null)
            {
                throw new ArgumentNullException("apiAccessToken");
            }

            bool hasPointsPersistence = false;
            InSkillProduct[] userProducts = null;

            // If unable to talk to InSkillProductClient, then set the HasPointsPersistence value to false and log the error.
            try
            {
                InSkillProductsClient client     = new InSkillProductsClient(skillRequest);
                InSkillProductsResponse response = await client.GetProducts();
                userProducts                     = response.Products;
                hasPointsPersistence             = userProducts.Any(x => 
                {
                    return  x.ProductId == RequestBusinessLogic.POINTS_PERSISTENCE_PRODUCT_ID && 
                            x.Entitled  == Entitlement.Entitled;
                });
            }
            catch(Exception e)
            {
                this.logger.LogError(e, "Unable to determine HasPointsPersistence. Setting value to false. RequestId: {0}, UserId: {1}.", skillRequest.Request.RequestId, skillRequest.Context.System.User.UserId);
            }
            

            this.logger.LogTrace("END HasPointsPersistence. RequestId: {0}, UserId: {1}, Products: {2}.", skillRequest.Request.RequestId, skillRequest.Context.System.User.UserId, userProducts == null ? "UNKNOWN" : JsonConvert.SerializeObject(userProducts));

            return hasPointsPersistence;
        }

        public async Task<TokenUser> GetUserApplicationState(SkillRequest input)
        {
            this.logger.LogTrace("BEGIN GetUserApplicationState. RequestId: {0}.", input.Request.RequestId);
            
            string userId = input.Context.System.User.UserId;

            TokenUser tokenUser = await this.tokenUserData.Get(userId);
            tokenUser = tokenUser ?? RequestBusinessLogic.GenerateEmptyTokenUser(userId);

            tokenUser.HasPointsPersistence = await this.HasPointsPersistence(input);

            this.logger.LogTrace("END GetUserApplicationState. RequestId: {0}, TokenUser: {1}.", input.Request.RequestId, JsonConvert.SerializeObject(TokenUser.GetCleansedTokenUser(tokenUser)));

            return tokenUser;
        }

        public async Task<SkillResponse> GetSkillResponse(SkillRequest input, TokenUser appUser)
        {
            this.logger.LogTrace("BEGIN GetSkillResponse. RequestId: {0}.", input.Request.RequestId);
            
            SkillResponse response = null;

            IRequestHandler requestHandler = this.GetRequestHandler(input);
            response = await requestHandler.GetSkillResponse(input, appUser);

            this.logger.LogTrace("END GetSkillResponse. RequestId: {0}.", input.Request.RequestId);
        
            return response;
        }

        public IRequestHandler GetRequestHandler(SkillRequest input)
        {
            this.logger.LogTrace("BEGIN GetRequestHandler. RequestId: {0}.", input.Request.RequestId);
            
            IRequestHandler requestHandler = null;
            
            if (input.Request is IntentRequest)
            {
                requestHandler = this.requestHandlers.FirstOrDefault(x => x is IntentRequestHandler);
            }
            else if (input.Request is ConnectionResponseRequest)
            {
                throw new NotSupportedException();
            }
            else if (input.Request is AccountLinkSkillEventRequest)
            {
                throw new NotSupportedException();
            }
            else if (input.Request is AudioPlayerRequest)
            {
                throw new NotSupportedException();
            }
            else if (input.Request is DisplayElementSelectedRequest)
            {
                throw new NotSupportedException();
            }
            else if (input.Request is IntentRequest)
            {
                throw new NotSupportedException();
            }
            else if (input.Request is LaunchRequest)
            {
                throw new NotSupportedException();
            }
            else if (input.Request is PermissionSkillEventRequest)
            {
                throw new NotSupportedException();
            }
            else if (input.Request is PlaybackControllerRequest)
            {
                throw new NotSupportedException();
            }
            else if (input.Request is SessionEndedRequest)
            {
                throw new NotSupportedException();
            }
            else if (input.Request is SkillEventRequest)
            {
                throw new NotSupportedException();
            }
            else if (input.Request is SystemExceptionRequest)
            {
                throw new NotSupportedException();
            }
            else
            {
                this.logger.LogWarning("Unidentified request type detected. Cannot route request type '{0}'.", input.Request.Type);
            }

            this.logger.LogTrace("END GetRequestHandler. RequestId: {0}. RequestHandler Type: '{1}'.", input.Request.RequestId, requestHandler.GetType().Name);

            return requestHandler;
        }

        public async Task<SkillResponse> HandleSkillRequest(SkillRequest skillRequest, ILambdaContext context)
        {
            this.logger.LogTrace("BEGIN Handling request type '{0}'. RequestId: {1}.", skillRequest.Request.Type, skillRequest.Request.RequestId);

            // Load the user's application state
            TokenUser appUser = await this.GetUserApplicationState(skillRequest);
            
            // Handle the request
            SkillResponse response = await this.GetSkillResponse(skillRequest, appUser);
            
            // Save the user's application state
            await this.tokenUserData.Save(appUser);
             
            this.logger.LogTrace("END Handling request type '{0}'. RequestId: {1}.", skillRequest.Request.Type, skillRequest.Request.RequestId);

            return response;
        }
    }
}