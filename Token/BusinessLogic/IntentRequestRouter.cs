
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using System;
using Microsoft.Extensions.Logging;
using Token.Models;
using Token.Core;
using System.Collections.Generic;
using System.Linq;
using Token.BusinessLogic.Interfaces;

namespace Token.BusinessLogic
{
  public class IntentRequestRouter : IRequestRouter
  {
    private IEnumerable<IIntentRequestHandler> intentRequestHandlers;

    private ILogger<IntentRequestRouter> logger;

    ISkillRequestValidator skillRequestValidator;

    public RequestType RequestType { get { return RequestType.IntentRequest; }}

    public IntentRequestRouter(ISkillRequestValidator skillRequestValidator, ILogger<IntentRequestRouter> logger, IEnumerable<IIntentRequestHandler> intentRequestHandlers)
    {
      if (skillRequestValidator == null)
      {
        throw new ArgumentNullException("skillRequestValidator");
      }
      
      if (logger == null)
      {
        throw new ArgumentNullException("logger");
      }

      if (intentRequestHandlers == null || intentRequestHandlers.Count() <= 0)
      {
        throw new ArgumentNullException("intentRequestHandlers");
      }

      this.skillRequestValidator = skillRequestValidator;
      this.logger = logger;
      this.intentRequestHandlers = intentRequestHandlers;
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

      IntentRequest intentRequest = skillRequest.Request as IntentRequest;

      if (intentRequest.Intent.ConfirmationStatus == "DENIED")
      {
        return string.Format("Okay").Tell();
      }

      // Get the right handler for the IntentRequest based on the name of the intent
      IIntentRequestHandler requestHandler = intentRequestHandlers.Where(x => x.IntentRequestHandlerName == intentRequest.Intent.Name).FirstOrDefault();

      if (requestHandler == null)
      {
        throw new NotSupportedException(string.Format("Cannot successfully route IntentRequest '{0}'.", intentRequest.Intent.Name));
      }

      // Handle the request
      SkillResponse speechResponse = await Task.Run(() => requestHandler.Handle(skillRequest, tokenUser));

      this.logger.LogTrace("END GetSkillResponse. RequestId: {0}.", skillRequest.Request.RequestId);

      return speechResponse;
    }
  }
}