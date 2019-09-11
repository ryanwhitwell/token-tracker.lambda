using Alexa.NET.Request;
using Alexa.NET.Response;
using Token.Models;

namespace Token.BusinessLogic.RequestHandlers
{
    public interface IIntentRequestHandler
    {
        SkillResponse AddPlayer(SkillRequest skillRequest, TokenUser tokenUser);
        SkillResponse AddPoints(SkillRequest skillRequest, TokenUser tokenUser);
        SkillResponse AddAllPoints(SkillRequest skillRequest, TokenUser tokenUser);
        SkillResponse RemovePoints(SkillRequest skillRequest, TokenUser tokenUser);
        SkillResponse AddSinglePoint(SkillRequest skillRequest, TokenUser tokenUser);
        SkillResponse RemoveSinglePoint(SkillRequest skillRequest, TokenUser tokenUser);
        SkillResponse ResetAllPoints(SkillRequest skillRequest, TokenUser tokenUser);
        SkillResponse ListAllPlayers(SkillRequest skillRequest, TokenUser tokenUser);
        SkillResponse ListAllPoints(SkillRequest skillRequest, TokenUser tokenUser);
        SkillResponse GetPlayerPoints(SkillRequest skillRequest, TokenUser tokenUser);
        SkillResponse GetPointsMax(SkillRequest skillRequest, TokenUser tokenUser);
        SkillResponse GetPointsMin(SkillRequest skillRequest, TokenUser tokenUser);
        SkillResponse GetPointsAverage(SkillRequest skillRequest, TokenUser tokenUser);
        SkillResponse DeletePlayer(SkillRequest skillRequest, TokenUser tokenUser);
        SkillResponse DeleteAllPlayers(SkillRequest skillRequest, TokenUser tokenUser);
        SkillResponse GetAllPlayersCount(SkillRequest skillRequest, TokenUser tokenUser);
        SkillResponse RemoveAllPoints(SkillRequest skillRequest, TokenUser tokenUser);
    }
}