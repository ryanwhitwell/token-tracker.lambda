using Alexa.NET.Request;

namespace Token.BusinessLogic.Interfaces
{
  public interface IRequestMapper
  {
    IRequestRouter GetRequestHandler(SkillRequest skillRequest);
  }
}