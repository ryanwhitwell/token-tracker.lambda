using System.Linq;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Microsoft.Extensions.Logging;
using Token.BusinessLogic.Interfaces;
using Token.Models;
using Token.Core;
using System;

namespace Token.BusinessLogic.IntentRequestHandlers
{
  public class AddPlayer : BaseRequestHandler<AddPlayer>, IIntentRequestHandler
  {
    public AddPlayer(ILogger<AddPlayer> logger, ISkillRequestValidator skillRequestValidator) : base(IntentRequestName.AddPlayer, logger, skillRequestValidator) { }

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
      
      logger.LogTrace("BEGIN AddPlayer. RequestId: {0}.", skillRequest.Request.RequestId);

      IntentRequest intentRequest = skillRequest.Request as IntentRequest;

      string playerName = Configuration.TEXT_INFO.ToTitleCase(intentRequest.Intent.Slots["player"].Value);

      Player existingPlayer = tokenUser.Players.FirstOrDefault(x => x.Name == playerName);

      SkillResponse response;
      if (existingPlayer != null)
      {
        // Don't update any data
        response = string.Format("{0} is already in your list of players.", existingPlayer.Name).Tell();
      }
      else
      {
        // Add new Player data
        tokenUser.Players.Add(new Player() { Name = playerName });
        response = string.Format("Alright, I added {0} to your list of players.", playerName).Tell();
      }

      logger.LogTrace("END AddPlayer. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }
}