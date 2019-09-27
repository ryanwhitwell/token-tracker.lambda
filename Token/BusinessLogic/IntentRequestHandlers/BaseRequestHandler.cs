using System;
using Microsoft.Extensions.Logging;

namespace Token.BusinessLogic.IntentRequestHandlers
{
  public abstract class BaseRequestHandler<T>
  {
    private ILogger<T> _logger;
    private ISkillRequestValidator _skillRequestValidator;
    private string _intentRequestName;

    public string IntentRequestHandlerName { get { return _intentRequestName; } }

    public BaseRequestHandler(string intentRequestHandlerName, ILogger<T> logger, ISkillRequestValidator skillRequestValidator)
    {
      if (String.IsNullOrWhiteSpace(intentRequestHandlerName))
      {
        throw new ArgumentNullException("intentRequestHandlerName");
      }
      
      if (logger == null)
      {
        throw new ArgumentNullException("logger");
      }

      if (skillRequestValidator == null)
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