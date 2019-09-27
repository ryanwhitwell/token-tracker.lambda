using System.Linq;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Microsoft.Extensions.Logging;
using Token.BusinessLogic.Interfaces;
using Token.Models;
using Token.Core;
using System.Text;
using System;

namespace Token.BusinessLogic.IntentRequestHandlers
{
  public class ListAllPlayers : BaseRequestHandler<ListAllPlayers>, IIntentRequestHandler
  {
    public string IntentRequestHandlerName { get { return IntentRequestName.ListAllPlayers; } }
    
    public ListAllPlayers(ILogger<ListAllPlayers> logger, ISkillRequestValidator skillRequestValidator) : base(logger, skillRequestValidator) { }

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
      
      logger.LogTrace("BEGIN ListAllPlayers. RequestId: {0}.", skillRequest.Request.RequestId);

      SkillResponse response = null;
      if (tokenUser.Players == null || tokenUser.Players.Count <= 0)
      {
        response = string.Format("Hmm, you don't have any players yet.").Tell();
      }
      else
      {
        StringBuilder responsePhraseBuilder = new StringBuilder();

        responsePhraseBuilder.Append("Okay, here we go. The players in your list are");

        string[] arrayOfPlayersNames = tokenUser.Players.Select(x => x.Name).ToArray();
        for (int i = 0; i < arrayOfPlayersNames.Length; i++)
        {
          if (i == 0)
          {
            responsePhraseBuilder.AppendFormat(" {0}", arrayOfPlayersNames[i]);
            continue;
          }

          if (i == arrayOfPlayersNames.Length - 1)
          {
            responsePhraseBuilder.AppendFormat(" and {0}.", arrayOfPlayersNames[i]);
            continue;
          }

          responsePhraseBuilder.AppendFormat(", {0}", arrayOfPlayersNames[i]);
        }

        responsePhraseBuilder.Append(" I think that's everybody.");

        response = responsePhraseBuilder.ToString().Tell();
      }

      logger.LogTrace("END ListAllPlayers. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }
}