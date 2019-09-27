using System.Linq;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Microsoft.Extensions.Logging;
using Token.BusinessLogic.Interfaces;
using Token.Models;
using Token.Core;
using System;

namespace Token.BusinessLogic.IntentRequestHandlers
{
  public class ResetAllPoints : BaseRequestHandler<ResetAllPoints>, IIntentRequestHandler
  {
    public ResetAllPoints(ILogger<ResetAllPoints> logger, ISkillRequestValidator skillRequestValidator) : base(logger, skillRequestValidator) { }

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
      
      logger.LogTrace("BEGIN ResetAllPoints. RequestId: {0}.", skillRequest.Request.RequestId);

      SkillResponse response;

      tokenUser.Players = tokenUser.Players.Select(x => new Player() { Name = x.Name, Points = 0 }).ToList();

      response = string.Format("Okay, I reset all of the players' points to zero.").Tell();

      logger.LogTrace("END ResetAllPoints. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }
}