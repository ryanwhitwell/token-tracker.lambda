using System;
using System.Linq;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Microsoft.Extensions.Logging;
using Token.BusinessLogic.Interfaces;
using Token.Models;
using Token.Core;

namespace Token.BusinessLogic.IntentRequestHandlers
{
  public class AddPoints : BaseRequestHandler<AddPoints>, IIntentRequestHandler
  {
    public AddPoints(ILogger<AddPoints> logger) : base(logger) { }

    public SkillResponse Handle(SkillRequest skillRequest, TokenUser tokenUser)
    {
      logger.LogTrace("BEGIN AddPoints. RequestId: {0}.", skillRequest.Request.RequestId);

      IntentRequest intentRequest = skillRequest.Request as IntentRequest;

      string playerName = Configuration.TEXT_INFO.ToTitleCase(intentRequest.Intent.Slots["player"].Value);
      int points = Int32.Parse(intentRequest.Intent.Slots["amount"].Value);

      Player existingPlayer = tokenUser.Players.FirstOrDefault(x => x.Name == playerName);

      string pointsResponseWord = points != Math.Abs(1) ? "points" : "point";

      SkillResponse response = null;
      if (existingPlayer != null)
      {
        // Remove old Player data
        tokenUser.Players = tokenUser.Players.Where(x => x.Name != existingPlayer.Name).ToList();

        // Add updated Player data
        existingPlayer.Points += points;
        tokenUser.Players.Add(existingPlayer);

        response = string.Format("Okay, I added {0} {1} to {2}.", points, pointsResponseWord, existingPlayer.Name).Tell();
      }
      else
      {
        // Add new Player data
        tokenUser.Players.Add(new Player() { Name = playerName, Points = points });
        response = string.Format("Alright, I added {0} to your list of players and gave them {1} {2}.", playerName, points, pointsResponseWord).Tell();
      }

      logger.LogTrace("END AddPoints. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }
}