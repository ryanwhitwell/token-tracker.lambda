using System;
using Alexa.NET.InSkillPricing.Responses;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Token.BusinessLogic.Interfaces;
using Token.Core;
using Token.Models;

namespace Token.BusinessLogic.ConnectionResponseRequestHandlers
{
  public class DefaultConnectionResponseRequest : BaseRequestHandler<DefaultConnectionResponseRequest>, IConnectionResponseRequestHandler
  {
    public string HandlerName { get { return ConnectionResponseRequestName.Default; } }
    
    public DefaultConnectionResponseRequest(ILogger<DefaultConnectionResponseRequest> logger, ISkillRequestValidator skillRequestValidator) : base(logger, skillRequestValidator) { }

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
      
      ConnectionResponseRequest request = skillRequest.Request as ConnectionResponseRequest;
      ConnectionResponsePayload payload = request.Payload as ConnectionResponsePayload;

      if (payload == null)
      {
        throw new ArgumentNullException("payload");
      }

      if (string.IsNullOrWhiteSpace(payload.PurchaseResult))
      {
        throw new ArgumentNullException("purchaseResult");
      }

      SkillResponse response;
      switch(payload.PurchaseResult)
      {
        case PurchaseResult.Accepted:
          tokenUser.HasPointsPersistence = true;
          response = string.Format("Great. I've turned on {0}. Have fun.", Configuration.File.GetSection("InSkillProducts").GetSection("PointsPersistence")["Name"]).Tell();
          break;
        case PurchaseResult.Declined:
          response = string.Format("No problem. You can ask me to sign up for {0} anytime.", Configuration.File.GetSection("InSkillProducts").GetSection("PointsPersistence")["Name"]).Tell();
          break;
        case PurchaseResult.Error:
          response = string.Format("I'm sorry. There was a problem handling the purchase of {0}.", Configuration.File.GetSection("InSkillProducts").GetSection("PointsPersistence")["Name"]).Tell();
          logger.LogError(string.Format("An error occurred while a user was attempting to purchase a product. User Id: {0}, Product Id: {1}, ConnectionResponsePayload: {2}.", tokenUser.Id,  Configuration.File.GetSection("InSkillProducts").GetSection("PointsPersistence")["Id"], JsonConvert.SerializeObject(payload)));
          break;
        case PurchaseResult.AlreadyPurchased:
          tokenUser.HasPointsPersistence = true;
          response = string.Format("Thanks. You are already subscribed to {0}.", Configuration.File.GetSection("InSkillProducts").GetSection("PointsPersistence")["Name"]).Tell();
          break;
        default:
          throw new NotSupportedException(string.Format("PurchaseResult '{0}' is not supported.", payload.PurchaseResult));
      }

      logger.LogTrace("END Default. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }
}