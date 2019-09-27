using Alexa.NET.Request;
using Alexa.NET.Response;
using Token.Models;

namespace Token.BusinessLogic.Interfaces
{
  public interface IIntentRequestHandler
  {
    SkillResponse Handle(SkillRequest skillRequest, TokenUser tokenUser);
    string IntentRequestHandlerName { get; }
  }
}