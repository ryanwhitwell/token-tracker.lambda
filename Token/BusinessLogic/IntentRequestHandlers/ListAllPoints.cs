using System;
using System.Linq;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Microsoft.Extensions.Logging;
using Token.BusinessLogic.Interfaces;
using Token.Models;
using Token.Core;
using System.Text;

namespace Token.BusinessLogic.IntentRequestHandlers
{
  public class ListAllPoints : BaseRequestHandler<ListAllPoints>, IIntentRequestHandler
  {
    public string HandlerName { get { return IntentRequestName.ListAllPoints; } }
    
    public ListAllPoints(ILogger<ListAllPoints> logger, ISkillRequestValidator skillRequestValidator) : base(logger, skillRequestValidator) { }

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
      
      logger.LogTrace("BEGIN ListAllPoints. RequestId: {0}.", skillRequest.Request.RequestId);

      SkillResponse response = null;
      if (tokenUser.Players == null || tokenUser.Players.Count <= 0)
      {
        response = string.Format("Hmm, you don't have any tokens yet.").Tell(true);
      }
      else if (tokenUser.Players.Count == 1)
      {
        string pointsWord = Math.Abs(tokenUser.Players[0].Points) != 1 ? "points" : "point";
        response = string.Format("Alright, it looks like the only token in your list is the color {0}, and they have {1} {2}.", tokenUser.Players[0].Name, tokenUser.Players[0].Points, pointsWord).Tell(true);
      }
      else
      {
        StringBuilder responsePhraseBuilder = new StringBuilder();
        responsePhraseBuilder.Append("Okay, here we go. From highest to lowest.");

        tokenUser.Players.OrderByDescending(x => x.Points).ToList().ForEach(x =>
        {
          string pointsWord = Math.Abs(x.Points) != 1 ? "points" : "point";
          responsePhraseBuilder.AppendFormat(" {0} has {1} {2}.", x.Name, x.Points, pointsWord);
        });

        responsePhraseBuilder.AppendFormat(" I think that's everybody.");

        response = responsePhraseBuilder.ToString().Tell(true);
      }

      logger.LogTrace("END ListAllPoints. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }
}