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
  public class GetPointsAverage : BaseRequestHandler<GetPointsAverage>, IIntentRequestHandler
  {
    public GetPointsAverage(ILogger<GetPointsAverage> logger) : base(logger) { }

    public SkillResponse Handle(SkillRequest skillRequest, TokenUser tokenUser)
    {
      logger.LogTrace("BEGIN GetPointsAverage. RequestId: {0}.", skillRequest.Request.RequestId);

      IntentRequest intentRequest = skillRequest.Request as IntentRequest;


      double[] allPoints = tokenUser.Players.Select(x => (double)x.Points).ToArray();
      double averagePoints = allPoints.Average();

      string pointsWord = Math.Abs(averagePoints) != 1 ? "points" : "point";
      SkillResponse response = string.Format("The average score for all players is {0} {1}.", averagePoints, pointsWord).Tell();

      logger.LogTrace("END GetPointsAverage. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }
}