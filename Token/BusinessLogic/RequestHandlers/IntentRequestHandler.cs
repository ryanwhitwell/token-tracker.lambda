
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using System;
using Microsoft.Extensions.Logging;
using Token.DataAccess.Interfaces;
using Token.Models;
using Newtonsoft.Json;

namespace Token.BusinessLogic.RequestHandlers
{
    public class IntentRequestHandler : IRequestHandler
    {
        private ILogger<IntentRequestHandler> logger;

        private ITokenUserData tokenUserData;

        public IntentRequestHandler (ILogger<IntentRequestHandler> logger, ITokenUserData tokenUserData)
        {
            if (logger is null)
            {
                throw new ArgumentNullException("logger");
            }

            if (tokenUserData is null)
            {
                throw new ArgumentNullException("tokenUserData");
            }

            this.logger = logger;
            this.tokenUserData = tokenUserData;
        }
        
        public async Task<SkillResponse> GetSkillResponse(SkillRequest skillRequest, TokenUser tokenUser)
        {
            this.logger.LogTrace("BEGIN GetSkillResponse. RequestId: {0}.", skillRequest.Request.RequestId);
            
            if (skillRequest is null)
            {
                throw new ArgumentNullException("skillRequest");
            }
            
            if (tokenUser is null)
            {
                throw new ArgumentNullException("tokenUser");
            }
            
            IntentRequest intentRequest = skillRequest.Request as IntentRequest;

            SsmlOutputSpeech speech = new SsmlOutputSpeech();
            speech.Ssml = string.Format("<speak>I'm handling the intent named {0}.</speak>", intentRequest.Intent.Name);
            this.logger.LogInformation(JsonConvert.SerializeObject(tokenUser));

            SkillResponse speechResponse = await Task.Run(() => ResponseBuilder.Tell(speech));

            this.logger.LogTrace("END GetSkillResponse. RequestId: {0}.", skillRequest.Request.RequestId);

            return speechResponse;
        }
    }
}