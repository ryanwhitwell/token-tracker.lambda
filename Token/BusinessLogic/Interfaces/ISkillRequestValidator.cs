using Alexa.NET.Request;

namespace Token.BusinessLogic
{
  public interface ISkillRequestValidator
  {
    bool IsValid(SkillRequest skillRequest);
  }
}