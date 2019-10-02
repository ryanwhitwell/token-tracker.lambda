
using Microsoft.Extensions.Logging;
using Token.Core;
using System.Collections.Generic;
using Token.BusinessLogic.Interfaces;

namespace Token.BusinessLogic
{
  public class ConnectionResponseRequestRouter : BaseRequestRouter<ConnectionResponseRequestRouter>
  {
    public ConnectionResponseRequestRouter(ISkillRequestValidator skillRequestValidator, ILogger<ConnectionResponseRequestRouter> logger, IEnumerable<ILaunchRequestHandler> requestHandlers) : base(RequestType.ConnectionResponseRequest, skillRequestValidator, logger, requestHandlers) { }
  }
}