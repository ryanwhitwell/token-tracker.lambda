using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Token.Models;

namespace Token.BusinessLogic
{
    public interface IRequestHandler
    {
        Task<SkillResponse> GetSkillResponse(SkillRequest skillRequest, TokenUser tokenUser);
    }
}