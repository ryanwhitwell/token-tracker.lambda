using System.Threading.Tasks;
using Alexa.NET.InSkillPricing;

namespace Token.BusinessLogic.Interfaces
{
  public interface ISkillProductsClient
  {
    Task<InSkillProductsResponse> GetProducts();
  }
}