using System;
using System.Linq;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Microsoft.Extensions.Logging;
using Token.BusinessLogic.Interfaces;
using Token.Models;
using Token.Core;
using System.Text;

namespace Token.BusinessLogic.IntentRequestHandlers
{
  public class GetPointsMin : BaseRequestHandler<GetPointsMin>, IIntentRequestHandler
  {
    public GetPointsMin(ILogger<GetPointsMin> logger, ISkillRequestValidator skillRequestValidator) : base(logger, skillRequestValidator) { }

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
      
      logger.LogTrace("BEGIN GetPointsMin. RequestId: {0}.", skillRequest.Request.RequestId);

      IntentRequest intentRequest = skillRequest.Request as IntentRequest;

      Player[] playersScoreAscending = tokenUser.Players.OrderBy(x => x.Points).ToArray();

      SkillResponse response;

      if (playersScoreAscending == null)
      {
        response = string.Format("Hmm, you don't see anyone in your list of players.").Tell();
      }
      else
      {
        int lowScore = playersScoreAscending[0].Points;
        Player[] lowScorePlayers = playersScoreAscending.Where(x => x.Points == lowScore).ToArray();

        if (lowScorePlayers.Length == playersScoreAscending.Count())
        {
          string pointsWord = Math.Abs(lowScore) != 1 ? "points" : "point";
          response = string.Format("All players are tied with a score of {0} {1}.", lowScore, pointsWord).Tell();
        }
        else if (lowScorePlayers.Length > 1)
        {
          StringBuilder responsePhraseBuilder = new StringBuilder();

          for (int i = 0; i < lowScorePlayers.Count(); i++)
          {
            Player currentPlayer = lowScorePlayers[i];
            if (i == 0)
            {
              responsePhraseBuilder.AppendFormat("{0}", currentPlayer.Name);
              continue;
            }

            responsePhraseBuilder.AppendFormat("and {0}", currentPlayer.Name);
          }

          string pointsWord = Math.Abs(lowScore) != 1 ? "points" : "point";
          responsePhraseBuilder.AppendFormat(" are tied for the lowest score with {0} {1}.", lowScore, pointsWord);

          response = responsePhraseBuilder.ToString().Tell();
        }
        else
        {
          string pointsWord = Math.Abs(lowScore) != 1 ? "points" : "point";
          response = string.Format("{0} has the lowest score with {1} {2}.", lowScorePlayers[0].Name, lowScore, pointsWord).Tell();
        }
      }

      logger.LogTrace("END GetPointsMin. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }
}