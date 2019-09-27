using System;
using Microsoft.Extensions.Logging;

namespace Token.BusinessLogic.IntentRequestHandlers
{
  public abstract class BaseRequestHandler<T>
  {
    private ILogger<T> _logger;
    private ISkillRequestValidator _skillRequestValidator;

    public BaseRequestHandler(ILogger<T> logger, ISkillRequestValidator skillRequestValidator)
    {
      if (logger is null)
      {
        throw new ArgumentNullException("logger");
      }

      if (skillRequestValidator is null)
      {
        throw new ArgumentNullException("skillRequestValidator");
      }

      _logger = logger;
      _skillRequestValidator = skillRequestValidator;
    }

    public ILogger<T> logger { get { return _logger; } }

    public ISkillRequestValidator skillRequestValidator { get { return _skillRequestValidator; } }
  }
}