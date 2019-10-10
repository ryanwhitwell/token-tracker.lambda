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
    public string HandlerName { get { return IntentRequestName.GetPointsAverage; } }
    
    public GetPointsAverage(ILogger<GetPointsAverage> logger, ISkillRequestValidator skillRequestValidator) : base(logger, skillRequestValidator) { }

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
      
      logger.LogTrace("BEGIN GetPointsAverage. RequestId: {0}.", skillRequest.Request.RequestId);

      IntentRequest intentRequest = skillRequest.Request as IntentRequest;

      double[] allPoints = tokenUser.Players.Select(x => (double)x.Points).ToArray();

      SkillResponse response;
      if (allPoints == null || allPoints.Length <= 0)
      {
        response = string.Format("Hmm, you don't see anyone in your list of tokens.").Tell(true);
      }
      else 
      {
        double averagePoints = allPoints.Average();
        string averagePointsFormatted = string.Format("{0:0.0}", averagePoints);

        string pointsWord = Math.Abs(averagePoints) != 1 ? "points" : "point";
        response = string.Format("The average score for all tokens is {0} {1}.", averagePointsFormatted, pointsWord).Tell(true);
      }

      logger.LogTrace("END GetPointsAverage. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }
}