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
  public class GetPointsMax : BaseRequestHandler<GetPointsMax>, IIntentRequestHandler
  {
    public string HandlerName { get { return IntentRequestName.GetPointsMax; } }
    
    public GetPointsMax(ILogger<GetPointsMax> logger, ISkillRequestValidator skillRequestValidator) : base(logger, skillRequestValidator) { }

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
      
      logger.LogTrace("BEGIN GetPointsMax. RequestId: {0}.", skillRequest.Request.RequestId);

      IntentRequest intentRequest = skillRequest.Request as IntentRequest;

      Player[] playersScoreDescending = tokenUser.Players.OrderByDescending(x => x.Points).ToArray();

      SkillResponse response;

      if (playersScoreDescending == null || playersScoreDescending.Length <= 0)
      {
        response = string.Format("Hmm, you don't see anyone in your list of tokens.").Tell(true);
      }
      else
      {
        int highScore = playersScoreDescending[0].Points;
        Player[] highScorePlayers = playersScoreDescending.Where(x => x.Points == highScore).ToArray();

        if (highScorePlayers.Length == playersScoreDescending.Count())
        {
          string pointsWord = Math.Abs(highScore) != 1 ? "points" : "point";
          response = string.Format("All tokens are tied with a score of {0} {1}.", highScore, pointsWord).Tell(true);
        }
        else if (highScorePlayers.Length > 1)
        {
          StringBuilder responsePhraseBuilder = new StringBuilder();

          for (int i = 0; i < highScorePlayers.Count(); i++)
          {
            Player currentPlayer = highScorePlayers[i];
            if (i == 0)
            {
              responsePhraseBuilder.AppendFormat("{0}", currentPlayer.Name);
              continue;
            }

            responsePhraseBuilder.AppendFormat(" and {0}", currentPlayer.Name);
          }

          string pointsWord = Math.Abs(highScore) != 1 ? "points" : "point";
          responsePhraseBuilder.AppendFormat(" are tied for the high score with {0} {1}.", highScore, pointsWord);

          response = responsePhraseBuilder.ToString().Tell(true);
        }
        else
        {
          string pointsWord = Math.Abs(highScore) != 1 ? "points" : "point";
          response = string.Format("{0} has the highest score with {1} {2}.", highScorePlayers[0].Name, highScore, pointsWord).Tell(true);
        }
      }

      logger.LogTrace("END GetPointsMax. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }
}