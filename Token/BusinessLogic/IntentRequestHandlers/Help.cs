using System.Linq;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Microsoft.Extensions.Logging;
using Token.BusinessLogic.Interfaces;
using Token.Models;
using Token.Core;
using System.Text;
using System;
using Alexa.NET.InSkillPricing.Directives;
using Alexa.NET;

namespace Token.BusinessLogic.IntentRequestHandlers
{
  public class Help : BaseRequestHandler<Help>, IIntentRequestHandler
  {
    public string HandlerName { get { return IntentRequestName.Help; } }
    
    public Help(ILogger<Help> logger, ISkillRequestValidator skillRequestValidator) : base(logger, skillRequestValidator) { }

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
      
      logger.LogTrace("BEGIN Help. RequestId: {0}.", skillRequest.Request.RequestId);

      SkillResponse response = string.Format("You can add a new token by saying something like, <emphasis>add</emphasis> a blue token. " +
      "To give a token points, you can say something like, <emphasis>give</emphasis> two points to red. " +
      "If you need more help, please check the instructions provided in the description of this skill in the Alexa skill catalog. So, what can I help you with?")
      .Tell(false);

      logger.LogTrace("END Help. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }
}