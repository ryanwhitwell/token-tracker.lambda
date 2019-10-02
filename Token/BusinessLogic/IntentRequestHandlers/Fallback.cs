using Alexa.NET.Request;
using Alexa.NET.Response;
using Microsoft.Extensions.Logging;
using Token.BusinessLogic.Interfaces;
using Token.Models;
using Token.Core;
using System;

namespace Token.BusinessLogic.IntentRequestHandlers
{
  public class Fallback : BaseRequestHandler<Fallback>, IIntentRequestHandler
  {
    public string HandlerName { get { return IntentRequestName.Fallback; } }
    
    public Fallback(ILogger<Fallback> logger, ISkillRequestValidator skillRequestValidator) : base(logger, skillRequestValidator) { }

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
      
      logger.LogTrace("BEGIN Fallback. RequestId: {0}.", skillRequest.Request.RequestId);

      SkillResponse response = string.Format("Hmm, I'm not sure what you wanted there.").TellWithReprompt("How can I help?");

      logger.LogTrace("END Fallback. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }
}