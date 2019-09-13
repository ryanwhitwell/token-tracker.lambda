using System.Linq;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Microsoft.Extensions.Logging;
using Token.BusinessLogic.Interfaces;
using Token.Models;
using Token.Core.StringExtensions;
using Token.Core;

namespace Token.BusinessLogic.IntentRequestHandlers
{
  public class RemoveSinglePoint : BaseRequestHandler<RemoveSinglePoint>, IIntentRequestHandler
  {
    public RemoveSinglePoint(ILogger<RemoveSinglePoint> logger) : base(logger) { }

    public SkillResponse Handle(SkillRequest skillRequest, TokenUser tokenUser)
    {
      logger.LogTrace("BEGIN RemoveSinglePoint. RequestId: {0}.", skillRequest.Request.RequestId);

      IntentRequest intentRequest = skillRequest.Request as IntentRequest;

      string playerName = Configuration.TEXT_INFO.ToTitleCase(intentRequest.Intent.Slots["player"].Value);

      Player existingPlayer = tokenUser.Players.FirstOrDefault(x => x.Name == playerName);

      SkillResponse response = null;
      if (existingPlayer != null)
      {
        // Remove old Player data
        tokenUser.Players = tokenUser.Players.Where(x => x.Name != existingPlayer.Name).ToList();

        // Add updated Player data
        existingPlayer.Points -= 1;
        tokenUser.Players.Add(existingPlayer);

        response = string.Format("Okay, I removed one point from {0}.", existingPlayer.Name).Tell();
      }
      else
      {
        response = string.Format("Hmm, I don't see {0} in your list of players.", playerName).Tell();
      }

      logger.LogTrace("END RemoveSinglePoint. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }
}