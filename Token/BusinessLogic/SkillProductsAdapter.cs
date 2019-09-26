using Alexa.NET;
using Alexa.NET.Request;
using Token.BusinessLogic.Interfaces;

namespace Token.BusinessLogic
{
  public class SkillProductsAdapter : ISkillProductsAdapter
  {
    public ISkillProductsClient GetClient(SkillRequest skillRequest)
    {
      InSkillProductsClient client = new InSkillProductsClient(skillRequest);

      return client as ISkillProductsClient;
    }
  }
}