using System;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Microsoft.Extensions.Logging;
using Token.BusinessLogic.Interfaces;
using Token.Core;
using Token.Models;

namespace Token.BusinessLogic.LaunchRequestHandlers
{
  public class DefaultLaunchRequest : BaseRequestHandler<DefaultLaunchRequest>, ILaunchRequestHandler
  {
    public string HandlerName { get { return LaunchRequestName.Default; } }
    
    public DefaultLaunchRequest(ILogger<DefaultLaunchRequest> logger, ISkillRequestValidator skillRequestValidator) : base(logger, skillRequestValidator) { }

    public SkillResponse Handle(SkillRequest skillRequest, TokenUser tokenUser)
    {
      if (tokenUser == null)
      {
        throw new ArgumentNullException("tokenUser");
      }
      
      if (tokenUser.HasPointsPersistence)
      {
        return string.Format("Welcome to {0}. You currently have Points Persistence. " +
        "To add a new token you can say something like, 'add the color blue', or to add points to an existing token, you can say something like, 'add three points to red'. " +
        "So, what can I help you with?", Configuration.File.GetSection("Application")["SkillName"])
        .TellWithReprompt(@"I didn't catch that. What can I help you with?");
      }

      return string.Format("Welcome to {0}. " +
        "To add a new token you can say something like, 'add the color blue', or to add points to an existing token, you can say something like, 'add three points to red'. " +
        "So, what can I help you with?", Configuration.File.GetSection("Application")["SkillName"])
        .TellWithReprompt(@"I didn't catch that. What can I help you with?");
    }
  }
}