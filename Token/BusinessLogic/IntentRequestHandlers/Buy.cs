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
  public class Buy : BaseRequestHandler<Buy>, IIntentRequestHandler
  {
    public string HandlerName { get { return IntentRequestName.Buy; } }
    
    public Buy(ILogger<Buy> logger, ISkillRequestValidator skillRequestValidator) : base(logger, skillRequestValidator) { }

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
      
      logger.LogTrace("BEGIN Buy. RequestId: {0}.", skillRequest.Request.RequestId);

      BuyDirective directive = new BuyDirective(Configuration.File.GetSection("InSkillProducts").GetSection("PointsPersistence")["Id"], "correlationToken");

      SkillResponse response = ResponseBuilder.Empty();
      response.Response.ShouldEndSession = false;

      response.Response.Directives.Add(directive);

      logger.LogTrace("END Buy. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }
}