using Alexa.NET;
using Alexa.NET.Request;
using Token.BusinessLogic.Interfaces;

namespace Token.BusinessLogic
{
  public class SkillProductsClientFactory : ISkillProductsClientFactory
  {
    public InSkillProductsClient Create(SkillRequest skillRequest)
    {
      return new InSkillProductsClient(skillRequest);
    }
  }
}