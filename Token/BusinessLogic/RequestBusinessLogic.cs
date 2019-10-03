using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexa.NET.InSkillPricing;
using Alexa.NET.InSkillPricing.Directives;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Token.BusinessLogic.Interfaces;
using Token.Core;
using Token.Data.Interfaces;
using Token.Models;

namespace Token.BusinessLogic
{
  public class RequestBusinessLogic : IRequestBusinessLogic
  {
    private static readonly string POINTS_PERSISTENCE_PRODUCT_ID = Configuration.File.GetSection("InSkillProducts").GetSection("PointsPersistence")["Id"];
    private ITokenUserData tokenUserData;
    private ILogger<RequestBusinessLogic> logger;
    private IRequestMapper requestMapper;
    private ISkillProductsClientAdapter skillProductsClientAdapter;
    ISkillRequestValidator skillRequestValidator;
    public RequestBusinessLogic(ISkillRequestValidator skillRequestValidator, ISkillProductsClientAdapter skillProductsClientAdapter, ILogger<RequestBusinessLogic> logger, IRequestMapper requestMapper, ITokenUserData tokenUserData)
    {
      if (skillRequestValidator == null)
      {
        throw new ArgumentNullException("skillRequestValidator");
      }
      
      if (skillProductsClientAdapter == null)
      {
        throw new ArgumentNullException("skillProductsAdapter");
      }

      if (logger == null)
      {
        throw new ArgumentNullException("logger");
      }

      if (requestMapper == null)
      {
        throw new ArgumentNullException("requestMapper");
      }

      if (tokenUserData == null)
      {
        throw new ArgumentNullException("tokenUserData");
      }

      this.skillRequestValidator = skillRequestValidator;
      this.skillProductsClientAdapter = skillProductsClientAdapter;
      this.logger = logger;
      this.requestMapper = requestMapper;
      this.tokenUserData = tokenUserData;
    }

    public static TokenUser GenerateEmptyTokenUser(string id)
    {
      if (String.IsNullOrWhiteSpace(id))
      {
        throw new ArgumentNullException("id");
      }

      TokenUser tokenUser = new TokenUser()
      {
        Id = id,
        Players = new List<Player>()
      };

      return tokenUser;
    }

    public async Task<bool> HasPointsPersistence(SkillRequest skillRequest)
    {
      if (!this.skillRequestValidator.IsValid(skillRequest))
      {
        throw new ArgumentNullException("skillRequest");
      }

      this.logger.LogTrace("BEGIN HasPointsPersistence. RequestId: {0}, UserId: {1}.", skillRequest.Request.RequestId, skillRequest.Context.System.User.UserId);

      bool hasPointsPersistence = false;
      InSkillProduct[] userProducts = null;

      // If unable to talk to InSkillProductClient, then set the HasPointsPersistence value to false and log the error.
      try
      {
        ISkillProductsClient client = this.skillProductsClientAdapter.GetClient(skillRequest);
        InSkillProductsResponse response = await client.GetProducts();

        if (response == null || response.Products == null)
        {
          return false;
        }

        userProducts = response.Products;
        hasPointsPersistence = userProducts.Any(x =>
        {
          return x.ProductId == RequestBusinessLogic.POINTS_PERSISTENCE_PRODUCT_ID &&
                 x.Entitled == Entitlement.Entitled;
        });
      }
      catch (Exception e)
      {
        this.logger.LogError(e, "Unable to determine HasPointsPersistence. Setting value to false. RequestId: {0}, UserId: {1}.", skillRequest.Request.RequestId, skillRequest.Context.System.User.UserId);
      }

      this.logger.LogTrace("END HasPointsPersistence. RequestId: {0}, UserId: {1}, Products: {2}.", skillRequest.Request.RequestId, skillRequest.Context.System.User.UserId, userProducts == null ? "UNKNOWN" : JsonConvert.SerializeObject(userProducts));

      return hasPointsPersistence;
    }

    public async Task<TokenUser> GetUserApplicationState(SkillRequest skillRequest)
    {
      if (!this.skillRequestValidator.IsValid(skillRequest))
      {
        throw new ArgumentNullException("skillRequest");
      }
      
      this.logger.LogTrace("BEGIN GetUserApplicationState. RequestId: {0}.", skillRequest.Request.RequestId);

      string userId = skillRequest.Context.System.User.UserId;

      TokenUser tokenUser = await this.tokenUserData.Get(userId);
      tokenUser = tokenUser ?? RequestBusinessLogic.GenerateEmptyTokenUser(userId);

      tokenUser.HasPointsPersistence = await this.HasPointsPersistence(skillRequest);

      this.logger.LogTrace("END GetUserApplicationState. RequestId: {0}, TokenUser: {1}.", skillRequest.Request.RequestId, JsonConvert.SerializeObject(tokenUser.Clean()));

      return tokenUser;
    }

    public async Task<SkillResponse> GetSkillResponse(SkillRequest skillRequest, TokenUser tokenUser)
    {
      if (!this.skillRequestValidator.IsValid(skillRequest))
      {
        throw new ArgumentNullException("skillRequest");
      }

      if (tokenUser == null)
      {
        throw new ArgumentNullException("tokenUser");
      }
      
      this.logger.LogTrace("BEGIN GetSkillResponse. RequestId: {0}.", skillRequest.Request.RequestId);

      IRequestRouter requestHandler = this.requestMapper.GetRequestHandler(skillRequest);
      SkillResponse response = response = await requestHandler.GetSkillResponse(skillRequest, tokenUser);

      this.logger.LogTrace("END GetSkillResponse. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }

    public void AddUpsellDirective(TokenUser tokenUser, SkillResponse response)
    {
      string message = string.Format("Your tokens and points will only be availble to use for a limited amount of time without a subscription to {0}. Do you want to know more?", Configuration.File.GetSection("InSkillProducts").GetSection("PointsPersistence")["Name"]);
      UpsellDirective directive = new UpsellDirective(Configuration.File.GetSection("InSkillProducts").GetSection("PointsPersistence")["Id"], "correlationToken", message);
      response.Response.Directives.Add(directive);
      tokenUser.UpsellTicks = 0;
    }

    public async Task<SkillResponse> HandleSkillRequest(SkillRequest skillRequest, ILambdaContext lambdaContext)
    {
      if (!this.skillRequestValidator.IsValid(skillRequest))
      {
        throw new ArgumentNullException("skillRequest");
      }

      if (lambdaContext == null)
      {
        throw new ArgumentNullException("lambdaContext");
      }
      
      this.logger.LogTrace("BEGIN Handling request type '{0}'. RequestId: {1}.", skillRequest.Request.Type, skillRequest.Request.RequestId);

      // Load the user's application state
      TokenUser tokenUser = await this.GetUserApplicationState(skillRequest);

      // Handle the request
      SkillResponse response = await this.GetSkillResponse(skillRequest, tokenUser);

      // Upsell if user doesn't have a subscription, they've reached the upsell tick threshold, and there isn't a reprompt in the response.
      if (response.Response.Reprompt == null &&
          (response.Response.Directives == null || response.Response.Directives.Count <= 0) &&
          !tokenUser.HasPointsPersistence && 
          tokenUser.UpsellTicks + 1 >= int.Parse(Configuration.File.GetSection("Application")["UpsellDirectiveInterval"]))
      {
        this.AddUpsellDirective(tokenUser, response);
      }
      else if (!tokenUser.HasPointsPersistence)
      {
        tokenUser.UpsellTicks++;
      }
      
      // Save the user's application state
      await this.tokenUserData.Save(tokenUser);

      this.logger.LogTrace("END Handling request type '{0}'. RequestId: {1}. Response: {2}", skillRequest.Request.Type, skillRequest.Request.RequestId, JsonConvert.SerializeObject(response));

      return response;
    }
  }
}