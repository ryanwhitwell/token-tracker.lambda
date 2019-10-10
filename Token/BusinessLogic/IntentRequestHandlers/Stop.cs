using Alexa.NET.Request;
using Alexa.NET.Response;
using Microsoft.Extensions.Logging;
using Token.BusinessLogic.Interfaces;
using Token.Models;
using Token.Core;
using System;

namespace Token.BusinessLogic.IntentRequestHandlers
{
  public class Stop : BaseRequestHandler<Stop>, IIntentRequestHandler
  {
    public string HandlerName { get { return IntentRequestName.Stop; } }
    
    public Stop(ILogger<Stop> logger, ISkillRequestValidator skillRequestValidator) : base(logger, skillRequestValidator) { }

    public SkillResponse Handle(SkillRequest skillRequest, TokenUser tokenUser)
    {
      if (!base.skillRequestValidator.IsValid(skillRequest))
      {
        throw new ArgumentNullException("skillRequest");
      }
      
      if (tokenUser == null)
      {
        throw new ArgumentNullException("tokenUser");
      }
      
      logger.LogTrace("BEGIN Stop. RequestId: {0}.", skillRequest.Request.RequestId);

      SkillResponse response = "Alright".Tell(true);

      logger.LogTrace("END Stop. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }
}