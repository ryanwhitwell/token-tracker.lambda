using System;
using System.Collections.Generic;
using System.Linq;
using Alexa.NET.InSkillPricing.Responses;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Token.BusinessLogic.Interfaces;
using Token.Core;

namespace Token.BusinessLogic
{
  public class RequestMapper : IRequestMapper
  {
    private IEnumerable<IRequestRouter> requestHandlers;

    private ILogger<RequestMapper> logger;

    ISkillRequestValidator skillRequestValidator;
    
    public RequestMapper(ISkillRequestValidator skillRequestValidator, ILogger<RequestMapper> logger, IEnumerable<IRequestRouter> requestHandlers)
    {
      if (skillRequestValidator is null)
      {
        throw new ArgumentNullException("skillRequestValidator");
      }
      
      if (logger is null)
      {
        throw new ArgumentNullException("logger");
      }

      if (requestHandlers is null || requestHandlers.Count() <= 0)
      {
        throw new ArgumentNullException("requestHandlers");
      }

      this.skillRequestValidator = skillRequestValidator;
      this.logger = logger;
      this.requestHandlers = requestHandlers;
    }
    
    public IRequestRouter GetRequestHandler(SkillRequest skillRequest)
    {
      if (!this.skillRequestValidator.IsValid(skillRequest))
      {
        throw new ArgumentNullException("skillRequest");
      }
      
      this.logger.LogTrace("BEGIN GetRequestHandler. RequestId: {0}.", skillRequest.Request.RequestId);

      this.logger.LogDebug(JsonConvert.SerializeObject(skillRequest));

      IRequestRouter requestHandler;

      if (skillRequest.Request is IntentRequest)
      {
        requestHandler = this.requestHandlers.FirstOrDefault(x => x.RequestType == RequestType.IntentRequest);
      }
      else if (skillRequest.Request is ConnectionResponseRequest)
      {
        throw new NotSupportedException();
      }
      else if (skillRequest.Request is AccountLinkSkillEventRequest)
      {
        throw new NotSupportedException();
      }
      else if (skillRequest.Request is AudioPlayerRequest)
      {
        throw new NotSupportedException();
      }
      else if (skillRequest.Request is DisplayElementSelectedRequest)
      {
        throw new NotSupportedException();
      }
      else if (skillRequest.Request is LaunchRequest)
      {
        throw new NotSupportedException();
      }
      else if (skillRequest.Request is PermissionSkillEventRequest)
      {
        throw new NotSupportedException();
      }
      else if (skillRequest.Request is PlaybackControllerRequest)
      {
        throw new NotSupportedException();
      }
      else if (skillRequest.Request is SessionEndedRequest)
      {
        throw new NotSupportedException();
      }
      else if (skillRequest.Request is SkillEventRequest)
      {
        throw new NotSupportedException();
      }
      else if (skillRequest.Request is SystemExceptionRequest)
      {
        throw new NotSupportedException();
      }
      else
      {
        throw new Exception(string.Format("Unidentified request type detected. Cannot route request type '{0}'.", skillRequest.Request.Type));
      }

      this.logger.LogTrace("END GetRequestHandler. RequestId: {0}. RequestHandler Type: '{1}'.", skillRequest.Request.RequestId, requestHandler.GetType().Name);

      return requestHandler;
    }
  }
}