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
    public ListAllPoints(ILogger<ListAllPoints> logger) : base(logger) { }

    public SkillResponse Handle(SkillRequest skillRequest, TokenUser tokenUser)
    {
      logger.LogTrace("BEGIN ListAllPoints. RequestId: {0}.", skillRequest.Request.RequestId);

      SkillResponse response = null;
      if (tokenUser.Players == null || tokenUser.Players.Count <= 0)
      {
        response = string.Format("Hmm, you don't have any players yet.").Tell();
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

        response = responsePhraseBuilder.ToString().Tell();
      }

      logger.LogTrace("END ListAllPoints. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }
}