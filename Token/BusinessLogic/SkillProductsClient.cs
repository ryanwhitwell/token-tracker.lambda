using System;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.InSkillPricing;
using Alexa.NET.Request;
using Token.BusinessLogic.Interfaces;

namespace Token.BusinessLogic
{
  public class SkillProductsClient : ISkillProductsClient
  {
    private InSkillProductsClient baseClient;
    
    public SkillProductsClient() {}
    
    public SkillProductsClient(SkillRequest request)
    {
      this.baseClient = new InSkillProductsClient(request);
    }
    
    public async virtual Task<InSkillProductsResponse> GetProducts()
    {
      return await this.baseClient.GetProducts();
    }
  }
}