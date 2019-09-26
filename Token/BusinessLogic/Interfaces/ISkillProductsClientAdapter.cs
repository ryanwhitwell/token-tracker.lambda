using Alexa.NET.Request;

namespace Token.BusinessLogic.Interfaces
{
  public interface ISkillProductsClientAdapter
  {
    ISkillProductsClient GetClient(SkillRequest skillRequest);
  }
}