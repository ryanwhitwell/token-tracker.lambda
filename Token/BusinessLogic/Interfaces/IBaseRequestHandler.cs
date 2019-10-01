using Alexa.NET.Request;
using Alexa.NET.Response;
using Token.Models;

namespace Token.BusinessLogic.Interfaces
{
  public interface IBaseRequestHandler
  {
    SkillResponse Handle(SkillRequest skillRequest, TokenUser tokenUser);
    string HandlerName { get; }
  }
}