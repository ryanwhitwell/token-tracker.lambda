using Alexa.NET.Request;
using Alexa.NET.Response;
using Microsoft.Extensions.Logging;
using Token.BusinessLogic.Interfaces;
using Token.Models;
using Token.Core.StringExtensions;
using System.Collections.Generic;

namespace Token.BusinessLogic.IntentRequestHandlers
{
  public class DeleteAllPlayers : BaseRequestHandler<DeleteAllPlayers>, IIntentRequestHandler
  {
    public DeleteAllPlayers(ILogger<DeleteAllPlayers> logger) : base(logger) { }

    public SkillResponse Handle(SkillRequest skillRequest, TokenUser tokenUser)
    {
      logger.LogTrace("BEGIN DeleteAllPlayers. RequestId: {0}.", skillRequest.Request.RequestId);

      SkillResponse response;

      tokenUser.Players = new List<Player>();

      response = string.Format("Alright, I removed everyone from your list of players.").Tell();

      logger.LogTrace("END DeleteAllPlayers. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }
}