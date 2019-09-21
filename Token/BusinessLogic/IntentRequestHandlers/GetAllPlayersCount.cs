using Alexa.NET.Request;
using Alexa.NET.Response;
using Microsoft.Extensions.Logging;
using Token.BusinessLogic.Interfaces;
using Token.Models;
using Token.Core;

namespace Token.BusinessLogic.IntentRequestHandlers
{
  public class GetAllPlayersCount : BaseRequestHandler<GetAllPlayersCount>, IIntentRequestHandler
  {
    public GetAllPlayersCount(ILogger<GetAllPlayersCount> logger) : base(logger) { }

    public SkillResponse Handle(SkillRequest skillRequest, TokenUser tokenUser)
    {
      logger.LogTrace("BEGIN GetAllPlayersCount. RequestId: {0}.", skillRequest.Request.RequestId);

      SkillResponse response;
      if (tokenUser.Players == null)
      {
        response = string.Format("There are no players your in your list.").Tell();
      }
      else
      {
        response = string.Format("There are {0} players your in your list.", tokenUser.Players.Count).Tell();
      }

      logger.LogTrace("END GetAllPlayersCount. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }
}