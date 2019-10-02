using Microsoft.Extensions.Logging;
using Token.Core;
using System.Collections.Generic;
using Token.BusinessLogic.Interfaces;

namespace Token.BusinessLogic
{
  public class LaunchRequestRouter : BaseRequestRouter<LaunchRequestRouter>
  {
    public LaunchRequestRouter(ISkillRequestValidator skillRequestValidator, ILogger<LaunchRequestRouter> logger, IEnumerable<ILaunchRequestHandler> requestHandlers) : base(RequestType.LaunchRequest, skillRequestValidator, logger, requestHandlers) { }
  }
}