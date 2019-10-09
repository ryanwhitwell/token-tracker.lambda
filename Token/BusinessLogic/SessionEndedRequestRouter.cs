using Microsoft.Extensions.Logging;
using Token.Core;
using System.Collections.Generic;
using Token.BusinessLogic.Interfaces;

namespace Token.BusinessLogic
{
  public class SessionEndedRequestRouter : BaseRequestRouter<SessionEndedRequestRouter>
  {
    public SessionEndedRequestRouter(ISkillRequestValidator skillRequestValidator, ILogger<SessionEndedRequestRouter> logger, IEnumerable<ISessionEndedRequestHandler> requestHandlers) : base(RequestType.SessionEndedRequest, skillRequestValidator, logger, requestHandlers) { }
  }
}