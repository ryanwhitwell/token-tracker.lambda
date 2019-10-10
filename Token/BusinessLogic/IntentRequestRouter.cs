
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

namespace Token.BusinessLogic
{
  public class IntentRequestRouter : BaseRequestRouter<IntentRequestRouter>
  {
    public IntentRequestRouter(ISkillRequestValidator skillRequestValidator, ILogger<IntentRequestRouter> logger, IEnumerable<IIntentRequestHandler> intentRequestHandlers) : base(RequestType.IntentRequest, skillRequestValidator, logger, intentRequestHandlers) { }

    public override async Task<SkillResponse> GetSkillResponse(SkillRequest skillRequest, TokenUser tokenUser)
    {
      if (!base.SkillRequestValidator.IsValid(skillRequest))
      {
        throw new ArgumentNullException("skillRequest");
      }

      if (tokenUser == null)
      {
        throw new ArgumentNullException("tokenUser");
      }
      
      base.Logger.LogTrace("BEGIN GetSkillResponse. RequestId: {0}.", skillRequest.Request.RequestId);

      IntentRequest intentRequest = skillRequest.Request as IntentRequest;

      if (intentRequest.Intent.ConfirmationStatus == "DENIED")
      {
        return string.Format("Okay").Tell(true);
      }

      // Get the right handler for the IntentRequest based on the name of the intent
      IIntentRequestHandler requestHandler = base.RequestHandlers.Where(x => x.HandlerName == intentRequest.Intent.Name).FirstOrDefault() as IIntentRequestHandler;

      if (requestHandler == null)
      {
        throw new NotSupportedException(string.Format("Cannot successfully route IntentRequest '{0}'.", intentRequest.Intent.Name));
      }

      // Handle the request
      SkillResponse skillResponse = await Task.Run(() => requestHandler.Handle(skillRequest, tokenUser));

      base.Logger.LogTrace("END GetSkillResponse. RequestId: {0}.", skillRequest.Request.RequestId);

      return skillResponse;
    }
  }
}