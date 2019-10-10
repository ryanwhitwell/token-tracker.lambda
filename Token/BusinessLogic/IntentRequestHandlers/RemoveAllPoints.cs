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
  public class RemoveAllPoints : BaseRequestHandler<RemoveAllPoints>, IIntentRequestHandler
  {
    public string HandlerName { get { return IntentRequestName.RemoveAllPoints; } }
    
    public RemoveAllPoints(ILogger<RemoveAllPoints> logger, ISkillRequestValidator skillRequestValidator) : base(logger, skillRequestValidator) { }

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
      
      logger.LogTrace("BEGIN RemoveAllPoints. RequestId: {0}.", skillRequest.Request.RequestId);

      IntentRequest intentRequest = skillRequest.Request as IntentRequest;

      int points = Int32.Parse(intentRequest.Intent.Slots["amount"].Value);

      SkillResponse response = null;
      if (tokenUser.Players == null || tokenUser.Players.Count <= 0)
      {
        response = string.Format("Hmm, I don't see anyone in your list of tokens.").Tell(true);
      }
      else
      {
        tokenUser.Players = tokenUser.Players.Select(x => new Player() { Name = x.Name, Points = x.Points - points }).ToList(); ;

        string pointsResponseWord = points != Math.Abs(1) ? "points" : "point";
        response = string.Format("Okay, I removed {0} {1} from all tokens.", points, pointsResponseWord).Tell(true);
      }

      logger.LogTrace("END RemoveAllPoints. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }
}