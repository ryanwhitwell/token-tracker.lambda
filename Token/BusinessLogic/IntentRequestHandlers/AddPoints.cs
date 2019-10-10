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
    public string HandlerName { get { return IntentRequestName.AddPoints; } }
    
    public AddPoints(ILogger<AddPoints> logger, ISkillRequestValidator skillRequestValidator) : base(logger, skillRequestValidator) { }

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
      
      logger.LogTrace("BEGIN AddPoints. RequestId: {0}.", skillRequest.Request.RequestId);

      IntentRequest intentRequest = skillRequest.Request as IntentRequest;

      string rawPlayerName = intentRequest.Intent.Slots["player"].Value;

      if (string.IsNullOrWhiteSpace(rawPlayerName))
      {
        throw new ArgumentException("Player cannot be null or whitespace.");
      }

      string playerName = Configuration.TEXT_INFO.ToTitleCase(intentRequest.Intent.Slots["player"].Value);

      string rawAmount = intentRequest.Intent.Slots["amount"].Value;

      if (string.IsNullOrWhiteSpace(rawAmount))
      {
        throw new ArgumentException("Amount cannot be null or whitespace.");
      }
      
      int points;
      if (!Int32.TryParse(intentRequest.Intent.Slots["amount"].Value, out points))
      {
        throw new ArgumentException(string.Format("Amount {0} cannot be converted to an integer.", rawAmount));
      }

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

        response = string.Format("Okay, I added {0} {1} to {2}.", points, pointsResponseWord, existingPlayer.Name).Tell(true);
      }
      else
      {
        // Add new Player data
        tokenUser.Players.Add(new Player() { Name = playerName, Points = points });
        response = string.Format("Alright, I added {0} to your list of tokens and gave them {1} {2}.", playerName, points, pointsResponseWord).Tell(true);
      }

      logger.LogTrace("END AddPoints. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }
}