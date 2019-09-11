using Alexa.NET.Request;
using Alexa.NET.Response;
using Token.Models;

namespace Token.BusinessLogic.RequestHandlers
{
    public interface IIntentRequestHandler
    {
        SkillResponse AddPoints(SkillRequest skillRequest, TokenUser tokenUser);
        SkillResponse AddPlayer(SkillRequest skillRequest, TokenUser tokenUser);
        SkillResponse RemovePlayer(SkillRequest skillRequest, TokenUser tokenUser);
        SkillResponse RemovePoints(SkillRequest skillRequest, TokenUser tokenUser);
        SkillResponse RemoveAllPlayers(SkillRequest skillRequest, TokenUser tokenUser);
        SkillResponse ListAllPlayers(SkillRequest skillRequest, TokenUser tokenUser);
        SkillResponse GetPlayerPoints(SkillRequest skillRequest, TokenUser tokenUser);
        SkillResponse RemoveAllPoints(SkillRequest skillRequest, TokenUser tokenUser);
        SkillResponse GetPointsMax(SkillRequest skillRequest, TokenUser tokenUser);
        SkillResponse GetPointsMin(SkillRequest skillRequest, TokenUser tokenUser);
        SkillResponse GetPointsAverage(SkillRequest skillRequest, TokenUser tokenUser);
        SkillResponse ListAllPoints(SkillRequest skillRequest, TokenUser tokenUser);
    }
}