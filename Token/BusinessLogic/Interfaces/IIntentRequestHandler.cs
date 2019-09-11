using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Token.Models;

namespace Token.BusinessLogic.RequestHandlers
{
    public interface IIntentRequestHandler
    {
        Task<SkillResponse> AddPoints(SkillRequest skillRequest, TokenUser tokenUser);
        Task<SkillResponse> AddPlayer(SkillRequest skillRequest, TokenUser tokenUser);
        Task<SkillResponse> RemovePlayer(SkillRequest skillRequest, TokenUser tokenUser);
        Task<SkillResponse> RemovePoints(SkillRequest skillRequest, TokenUser tokenUser);
        Task<SkillResponse> RemoveAllPlayers(SkillRequest skillRequest, TokenUser tokenUser);
        Task<SkillResponse> ListAllPlayers(SkillRequest skillRequest, TokenUser tokenUser);
        Task<SkillResponse> GetPlayerPoints(SkillRequest skillRequest, TokenUser tokenUser);
        Task<SkillResponse> RemoveAllPoints(SkillRequest skillRequest, TokenUser tokenUser);
        Task<SkillResponse> GetPointsMax(SkillRequest skillRequest, TokenUser tokenUser);
        Task<SkillResponse> GetPointsMin(SkillRequest skillRequest, TokenUser tokenUser);
        Task<SkillResponse> GetPointsAverage(SkillRequest skillRequest, TokenUser tokenUser);
        Task<SkillResponse> ListAllPoints(SkillRequest skillRequest, TokenUser tokenUser);
    }
}