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
  public class GetPlayerPoints : BaseRequestHandler<GetPlayerPoints>, IIntentRequestHandler
  {
    public string HandlerName { get { return IntentRequestName.GetPlayerPoints; } }
    
    public GetPlayerPoints(ILogger<GetPlayerPoints> logger, ISkillRequestValidator skillRequestValidator) : base(logger, skillRequestValidator) { }

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
      
      logger.LogTrace("BEGIN GetPlayerPoints. RequestId: {0}.", skillRequest.Request.RequestId);

      IntentRequest intentRequest = skillRequest.Request as IntentRequest;

      string playerName = Configuration.TEXT_INFO.ToTitleCase(intentRequest.Intent.Slots["player"].Value);

      Player existingPlayer = tokenUser.Players.FirstOrDefault(x => x.Name == playerName);

      SkillResponse response;
      if (existingPlayer != null)
      {
        string pointsWord = Math.Abs(existingPlayer.Points) != 1 ? "points" : "point";
        response = string.Format("{0} has {1} {2}.", existingPlayer.Name, existingPlayer.Points, pointsWord).Tell();
      }
      else
      {
        response = string.Format("Hmm, I don't see {0} in your list of players.", playerName).Tell();
      }

      logger.LogTrace("END GetPlayerPoints. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }
}