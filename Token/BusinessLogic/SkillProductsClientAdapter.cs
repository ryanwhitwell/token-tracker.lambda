using System;
using Alexa.NET;
using Alexa.NET.Request;
using Token.BusinessLogic.Interfaces;

namespace Token.BusinessLogic
{
  public class SkillProductsClientAdapter : ISkillProductsClientAdapter
  {
    ISkillRequestValidator skillRequestValidator;
    
    public SkillProductsClientAdapter(ISkillRequestValidator skillRequestValidator)
    {
      if (skillRequestValidator == null)
      {
        throw new ArgumentNullException("skillRequestValidator");
      }

      this.skillRequestValidator = skillRequestValidator;
    }
    
    public ISkillProductsClient GetClient(SkillRequest skillRequest)
    {
      if (!this.skillRequestValidator.IsValid(skillRequest))
      {
        throw new ArgumentNullException("skillRequest");
      }
      
      ISkillProductsClient client = new SkillProductsClient(skillRequest);

      return client;
    }
  }
}