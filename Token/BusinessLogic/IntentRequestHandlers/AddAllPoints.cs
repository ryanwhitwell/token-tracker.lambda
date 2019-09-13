using System;
using System.Linq;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Microsoft.Extensions.Logging;
using Token.BusinessLogic.Interfaces;
using Token.Models;
using Token.Core.StringExtensions;

namespace Token.BusinessLogic.IntentRequestHandlers
{
  public class AddAllPoints : BaseRequestHandler<AddAllPoints>, IIntentRequestHandler
  {
    public AddAllPoints(ILogger<AddAllPoints> logger) : base(logger) { }

    public SkillResponse Handle(SkillRequest skillRequest, TokenUser tokenUser)
    {
      logger.LogTrace("BEGIN AddAllPoints. RequestId: {0}.", skillRequest.Request.RequestId);

      IntentRequest intentRequest = skillRequest.Request as IntentRequest;

      int points = Int32.Parse(intentRequest.Intent.Slots["amount"].Value);

      SkillResponse response = null;
      if (tokenUser.Players == null || tokenUser.Players.Count <= 0)
      {
        response = string.Format("Hmm, I don't see anyone in your list of players.").Tell();
      }
      else
      {
        tokenUser.Players = tokenUser.Players.Select(x => new Player() { Name = x.Name, Points = x.Points + points }).ToList();

        string pointsResponseWord = points != Math.Abs(1) ? "points" : "point";
        response = string.Format("Okay, I gave all players {0} {1}.", points, pointsResponseWord).Tell();
      }

      logger.LogTrace("END AddAllPoints. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }
}