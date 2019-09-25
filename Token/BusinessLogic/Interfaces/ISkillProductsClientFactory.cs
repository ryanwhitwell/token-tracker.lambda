using Alexa.NET;
using Alexa.NET.Request;

namespace Token.BusinessLogic.Interfaces
{
  interface ISkillProductsClientFactory
  {
    InSkillProductsClient Create(SkillRequest skillRequest);
  }
}