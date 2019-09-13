using System;
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
  public class RemovePoints : BaseRequestHandler<RemovePoints>, IIntentRequestHandler
  {
    public RemovePoints(ILogger<RemovePoints> logger) : base(logger) { }

    public SkillResponse Handle(SkillRequest skillRequest, TokenUser tokenUser)
    {
      logger.LogTrace("BEGIN RemovePoints. RequestId: {0}.", skillRequest.Request.RequestId);

      IntentRequest intentRequest = skillRequest.Request as IntentRequest;

      string playerName = Configuration.TEXT_INFO.ToTitleCase(intentRequest.Intent.Slots["player"].Value);
      int points = Int32.Parse(intentRequest.Intent.Slots["amount"].Value);

      Player existingPlayer = tokenUser.Players.FirstOrDefault(x => x.Name == playerName);

      SkillResponse response = null;
      if (existingPlayer != null)
      {
        // Remove old Player data
        tokenUser.Players = tokenUser.Players.Where(x => x.Name != existingPlayer.Name).ToList();

        // Add updated Player data, remove points
        existingPlayer.Points -= points;
        tokenUser.Players.Add(existingPlayer);

        string pointsResponseWord = points != Math.Abs(1) ? "points" : "point";
        response = string.Format("Okay, I removed {0} {1} from {2}.", points, pointsResponseWord, existingPlayer.Name).Tell();
      }
      else
      {
        response = string.Format("Hmm, I don't see {0} in your list of players.", playerName).Tell();
      }

      logger.LogTrace("END RemovePoints. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }
}