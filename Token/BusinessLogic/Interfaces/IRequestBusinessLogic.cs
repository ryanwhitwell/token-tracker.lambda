using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using Token.Models;

namespace Token.BusinessLogic
{
    public interface IRequestBusinessLogic
    {
        Task<SkillResponse> HandleSkillRequest(SkillRequest skillRequest, ILambdaContext context);
        Task<TokenUser> GetUserApplicationState(SkillRequest skillRequest);
        Task<SkillResponse> GetSkillResponse(SkillRequest skillRequest, TokenUser tokenUser);
    }
}