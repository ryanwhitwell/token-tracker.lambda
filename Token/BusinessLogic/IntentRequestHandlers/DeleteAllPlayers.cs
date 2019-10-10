using Alexa.NET.Request;
using Alexa.NET.Response;
using Microsoft.Extensions.Logging;
using Token.BusinessLogic.Interfaces;
using Token.Models;
using Token.Core;
using System.Collections.Generic;
using System;

namespace Token.BusinessLogic.IntentRequestHandlers
{
  public class DeleteAllPlayers : BaseRequestHandler<DeleteAllPlayers>, IIntentRequestHandler
  {
    public string HandlerName { get { return IntentRequestName.DeleteAllPlayers; } }
    
    public DeleteAllPlayers(ILogger<DeleteAllPlayers> logger, ISkillRequestValidator skillRequestValidator) : base(logger, skillRequestValidator) { }

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
      
      logger.LogTrace("BEGIN DeleteAllPlayers. RequestId: {0}.", skillRequest.Request.RequestId);

      SkillResponse response;

      tokenUser.Players = new List<Player>();

      response = string.Format("Alright, I removed everyone from your list of tokens.").Tell(true);

      logger.LogTrace("END DeleteAllPlayers. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }
}