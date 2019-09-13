using System.Linq;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Microsoft.Extensions.Logging;
using Token.BusinessLogic.Interfaces;
using Token.Models;
using Token.Core.StringExtensions;
using Token.Core;
using System.Text;

namespace Token.BusinessLogic.IntentRequestHandlers
{
  public class DeletePlayer : BaseRequestHandler<DeletePlayer>, IIntentRequestHandler
  {
    public DeletePlayer(ILogger<DeletePlayer> logger) : base(logger) { }

    public SkillResponse Handle(SkillRequest skillRequest, TokenUser tokenUser)
    {
      logger.LogTrace("BEGIN DeletePlayer. RequestId: {0}.", skillRequest.Request.RequestId);

      IntentRequest intentRequest = skillRequest.Request as IntentRequest;

      string playerName = Configuration.TEXT_INFO.ToTitleCase(intentRequest.Intent.Slots["player"].Value);

      Player existingPlayer = tokenUser.Players.FirstOrDefault(x => x.Name == playerName);

      StringBuilder responsePhraseBuilder = new StringBuilder();

      SkillResponse response = null;
      if (existingPlayer == null)
      {
        response = string.Format("Hmm, I don't see {0} in your list of players.", playerName).Tell();
      }
      else
      {
        // Remove Player from list
        tokenUser.Players = tokenUser.Players.Where(x => x.Name != existingPlayer.Name).ToList();

        response = string.Format("Okay, I removed {0} from your list of players.", playerName).Tell();
      }

      logger.LogTrace("END DeletePlayer. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }
}