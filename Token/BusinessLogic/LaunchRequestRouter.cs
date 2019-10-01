
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
using Newtonsoft.Json;

namespace Token.BusinessLogic
{
  public class LaunchRequestRouter : IRequestRouter
  {
    private IEnumerable<ILaunchRequestHandler> requestHandlers;

    private ILogger<LaunchRequestRouter> logger;

    ISkillRequestValidator skillRequestValidator;

    public RequestType RequestType { get { return RequestType.IntentRequest; }}

    public LaunchRequestRouter(ISkillRequestValidator skillRequestValidator, ILogger<LaunchRequestRouter> logger, IEnumerable<ILaunchRequestHandler> requestHandlers)
    {
      if (skillRequestValidator == null)
      {
        throw new ArgumentNullException("skillRequestValidator");
      }
      
      if (logger == null)
      {
        throw new ArgumentNullException("logger");
      }

      if (requestHandlers == null || requestHandlers.Count() <= 0)
      {
        throw new ArgumentNullException("requestHandlers");
      }

      this.skillRequestValidator = skillRequestValidator;
      this.logger = logger;
      this.requestHandlers = requestHandlers;
    }

    public async Task<SkillResponse> GetSkillResponse(SkillRequest skillRequest, TokenUser tokenUser)
    {
      if (!this.skillRequestValidator.IsValid(skillRequest))
      {
        throw new ArgumentNullException("skillRequest");
      }

      if (tokenUser == null)
      {
        throw new ArgumentNullException("tokenUser");
      }
      
      this.logger.LogTrace("BEGIN GetSkillResponse. RequestId: {0}.", skillRequest.Request.RequestId);

      LaunchRequest launchRequest = skillRequest.Request as LaunchRequest;

      // There is only one handler and it is the default. Select it and throw an error if it's not bound correctly.
      ILaunchRequestHandler requestHandler = requestHandlers.FirstOrDefault();

      if (requestHandler == null)
      {
        throw new Exception(string.Format("Cannot successfully route LaunchRequest. The request handler is not bound to the container.", launchRequest.Type));
      }

      // Handle the request
      SkillResponse skillResponse = await Task.Run(() => requestHandler.Handle(skillRequest, tokenUser));

      this.logger.LogTrace("END GetSkillResponse. RequestId: {0}.", skillRequest.Request.RequestId);

      return skillResponse;
    }
  }
}