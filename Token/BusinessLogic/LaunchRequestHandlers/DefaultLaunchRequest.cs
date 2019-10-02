using System;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Microsoft.Extensions.Logging;
using Token.BusinessLogic.Interfaces;
using Token.Core;
using Token.Data;
using Token.Models;

namespace Token.BusinessLogic.LaunchRequestHandlers
{
  public class DefaultLaunchRequest : BaseRequestHandler<DefaultLaunchRequest>, ILaunchRequestHandler
  {
    public string HandlerName { get { return LaunchRequestName.Default; } }
    
    public DefaultLaunchRequest(ILogger<DefaultLaunchRequest> logger, ISkillRequestValidator skillRequestValidator) : base(logger, skillRequestValidator) { }

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

      logger.LogTrace("BEGIN Default. RequestId: {0}.", skillRequest.Request.RequestId);
      
      SkillResponse response;
      if (tokenUser.HasPointsPersistence)
      {
        response = string.Format("Welcome to {0}. You currently have Points Persistence so your tokens and points are available for as long as your subscription is active. " +
          "To add a new token you can say something like, <emphasis>add</emphasis> the color blue, or to add points to an existing token, you can say something like, " +
          "<emphasis>add</emphasis> three points to red. ", Configuration.File.GetSection("Application")["SkillName"])
          .TellWithReprompt("So, what can I help you with?");
      }
      else
      {
        response = string.Format("Welcome to {0}. Your tokens and points will be available to use for about the next {1} minutes. " +
          "To add a new token you can say something like, <emphasis>add</emphasis> the color blue, or to add points to an existing token, " +
          "you can say something like, <emphasis>add</emphasis> three points to red. ", Configuration.File.GetSection("Application")["SkillName"], tokenUser.DataTtlMinutes)
          .TellWithReprompt("So, what can I help you with?");
      }

      logger.LogTrace("END Default. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }
}