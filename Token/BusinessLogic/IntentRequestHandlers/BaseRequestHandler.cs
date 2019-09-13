using System;
using Microsoft.Extensions.Logging;

namespace Token.BusinessLogic.IntentRequestHandlers
{
  public abstract class BaseRequestHandler<T>
  {
    private ILogger<T> _logger;

    public BaseRequestHandler(ILogger<T> logger)
    {
      if (logger is null)
      {
        throw new ArgumentNullException("logger");
      }

      _logger = logger;
    }

    public ILogger<T> logger { get { return _logger; } }
  }
}