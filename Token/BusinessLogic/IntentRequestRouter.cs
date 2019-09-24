
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using System;
using Microsoft.Extensions.Logging;
using Token.Models;
using Token.Core;
using System.Collections.Generic;
using System.Linq;
using Token.BusinessLogic.Interfaces;
using Token.BusinessLogic.IntentRequestHandlers;

namespace Token.BusinessLogic
{
  public class IntentRequestRouter : IRequestRouter
  {
    private IEnumerable<IIntentRequestHandler> intentRequestHandlers;

    private ILogger<IntentRequestRouter> logger;

    public IntentRequestRouter(ILogger<IntentRequestRouter> logger, IEnumerable<IIntentRequestHandler> intentRequestHandlers)
    {
      if (logger is null)
      {
        throw new ArgumentNullException("logger");
      }

      if (intentRequestHandlers is null || intentRequestHandlers.Count() <= 0)
      {
        throw new ArgumentNullException("intentRequestHandlers");
      }

      this.logger = logger;
      this.intentRequestHandlers = intentRequestHandlers;
    }

    public async Task<SkillResponse> GetSkillResponse(SkillRequest skillRequest, TokenUser tokenUser)
    {
      this.logger.LogTrace("BEGIN GetSkillResponse. RequestId: {0}.", skillRequest.Request.RequestId);

      if (skillRequest is null)
      {
        throw new ArgumentNullException("skillRequest");
      }

      if (tokenUser is null)
      {
        throw new ArgumentNullException("tokenUser");
      }

      IntentRequest intentRequest = skillRequest.Request as IntentRequest;

      if (intentRequest.Intent.ConfirmationStatus == "DENIED")
      {
        return string.Format("Okay").Tell();
      }

      SkillResponse speechResponse = null;
      IIntentRequestHandler requestHandler = null;

      // Determine the route the request
      switch (intentRequest.Intent.Name)
      {
        case IntentRequestName.AddPoints:
          requestHandler = intentRequestHandlers.Where(x => x is AddPoints).FirstOrDefault();
          break;
        case IntentRequestName.AddPlayer:
          requestHandler = intentRequestHandlers.Where(x => x is AddPlayer).FirstOrDefault();
          break;
        case IntentRequestName.DeletePlayer:
          requestHandler = intentRequestHandlers.Where(x => x is DeletePlayer).FirstOrDefault();
          break;
        case IntentRequestName.RemovePoints:
          requestHandler = intentRequestHandlers.Where(x => x is RemovePoints).FirstOrDefault();
          break;
        case IntentRequestName.DeleteAllPlayers:
          requestHandler = intentRequestHandlers.Where(x => x is DeleteAllPlayers).FirstOrDefault();
          break;
        case IntentRequestName.ListAllPlayers:
          requestHandler = intentRequestHandlers.Where(x => x is ListAllPlayers).FirstOrDefault();
          break;
        case IntentRequestName.GetPlayerPoints:
          requestHandler = intentRequestHandlers.Where(x => x is GetPlayerPoints).FirstOrDefault();
          break;
        case IntentRequestName.ResetAllPoints:
          requestHandler = intentRequestHandlers.Where(x => x is ResetAllPoints).FirstOrDefault();
          break;
        case IntentRequestName.GetPointsMax:
          requestHandler = intentRequestHandlers.Where(x => x is GetPointsMax).FirstOrDefault();
          break;
        case IntentRequestName.GetPointsMin:
          requestHandler = intentRequestHandlers.Where(x => x is GetPointsMin).FirstOrDefault();
          break;
        case IntentRequestName.GetPointsAverage:
          requestHandler = intentRequestHandlers.Where(x => x is GetPointsAverage).FirstOrDefault();
          break;
        case IntentRequestName.ListAllPoints:
          requestHandler = intentRequestHandlers.Where(x => x is ListAllPoints).FirstOrDefault();
          break;
        case IntentRequestName.GetAllPlayersCount:
          requestHandler = intentRequestHandlers.Where(x => x is GetAllPlayersCount).FirstOrDefault();
          break;
        case IntentRequestName.AddAllPoints:
          requestHandler = intentRequestHandlers.Where(x => x is AddAllPoints).FirstOrDefault();
          break;
        case IntentRequestName.AddSinglePoint:
          requestHandler = intentRequestHandlers.Where(x => x is AddSinglePoint).FirstOrDefault();
          break;
        case IntentRequestName.RemoveSinglePoint:
          requestHandler = intentRequestHandlers.Where(x => x is RemoveSinglePoint).FirstOrDefault();
          break;
        case IntentRequestName.RemoveAllPoints:
          requestHandler = intentRequestHandlers.Where(x => x is RemoveAllPoints).FirstOrDefault();
          break;
        default:
          throw new NotSupportedException(string.Format("Cannot successfully route IntentRequest '{0}'.", intentRequest.Intent.Name));
      }

      // Handle the request
      speechResponse = await Task.Run(() => requestHandler.Handle(skillRequest, tokenUser));

      this.logger.LogTrace("END GetSkillResponse. RequestId: {0}.", skillRequest.Request.RequestId);

      return speechResponse;
    }
  }
}