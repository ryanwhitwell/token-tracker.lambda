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
  public class RefundSubscription : BaseRequestHandler<RefundSubscription>, IIntentRequestHandler
  {
    public string HandlerName { get { return IntentRequestName.RefundSubscription; } }
    
    public RefundSubscription(ILogger<RefundSubscription> logger, ISkillRequestValidator skillRequestValidator) : base(logger, skillRequestValidator) { }

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
      
      logger.LogTrace("BEGIN RefundSubscription. RequestId: {0}.", skillRequest.Request.RequestId);

      CancelDirective directive = new CancelDirective(Configuration.File.GetSection("InSkillProducts").GetSection("PointsPersistence")["Id"], "correlationToken");

      SkillResponse response = ResponseBuilder.Empty();
      response.Response.ShouldEndSession = true;

      response.Response.Directives.Add(directive);

      logger.LogTrace("END RefundSubscription. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }
}