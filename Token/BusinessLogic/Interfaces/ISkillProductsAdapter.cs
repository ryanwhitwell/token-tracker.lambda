using Alexa.NET.Request;

namespace Token.BusinessLogic.Interfaces
{
  public interface ISkillProductsAdapter
  {
    ISkillProductsClient GetClient(SkillRequest skillRequest);
  }
}