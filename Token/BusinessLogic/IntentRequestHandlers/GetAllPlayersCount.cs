using Alexa.NET.Request;
using Alexa.NET.Response;
using Microsoft.Extensions.Logging;
using Token.BusinessLogic.Interfaces;
using Token.Models;
using Token.Core;
using System;

namespace Token.BusinessLogic.IntentRequestHandlers
{
  public class GetAllPlayersCount : BaseRequestHandler<GetAllPlayersCount>, IIntentRequestHandler
  {
    public string HandlerName { get { return IntentRequestName.GetAllPlayersCount; } }
    
    public GetAllPlayersCount(ILogger<GetAllPlayersCount> logger, ISkillRequestValidator skillRequestValidator) : base(logger, skillRequestValidator) { }

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
      
      logger.LogTrace("BEGIN GetAllPlayersCount. RequestId: {0}.", skillRequest.Request.RequestId);

      SkillResponse response;
      if (tokenUser.Players == null)
      {
        response = string.Format("There are no tokens your in your list.").Tell();
      }
      else
      {
        response = string.Format("There are {0} tokens your in your list.", tokenUser.Players.Count).Tell();
      }

      logger.LogTrace("END GetAllPlayersCount. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }
}