using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Token.Models;

namespace Token.BusinessLogic.Interfaces
{
  public interface IRequestRouter
  {
    Task<SkillResponse> GetSkillResponse(SkillRequest skillRequest, TokenUser tokenUser);
  }
}