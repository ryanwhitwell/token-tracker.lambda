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
  public class GetPlayerPoints : BaseRequestHandler<GetPlayerPoints>, IIntentRequestHandler
  {
    public GetPlayerPoints(ILogger<GetPlayerPoints> logger) : base(logger) { }

    public SkillResponse Handle(SkillRequest skillRequest, TokenUser tokenUser)
    {
      logger.LogTrace("BEGIN GetPlayerPoints. RequestId: {0}.", skillRequest.Request.RequestId);

      IntentRequest intentRequest = skillRequest.Request as IntentRequest;

      string playerName = Configuration.TEXT_INFO.ToTitleCase(intentRequest.Intent.Slots["player"].Value);

      Player existingPlayer = tokenUser.Players.FirstOrDefault(x => x.Name == playerName);

      SkillResponse response;
      if (existingPlayer != null)
      {
        string pointsWord = Math.Abs(existingPlayer.Points) != 1 ? "points" : "point";
        response = string.Format("{0} has {1} {2}.", existingPlayer.Name, existingPlayer.Points, pointsWord).Tell();
      }
      else
      {
        response = string.Format("Hmm, I don't see {0} in your list of players.", playerName).Tell();
      }

      logger.LogTrace("END GetPlayerPoints. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }
}