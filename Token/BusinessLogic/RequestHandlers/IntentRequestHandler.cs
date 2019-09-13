
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using System;
using Microsoft.Extensions.Logging;
using Token.DataAccess.Interfaces;
using Token.Models;
using Token.Core;
using System.Collections.Generic;
using System.Linq;
using Token.Core.StringExtensions;
using System.Globalization;
using System.Text;

namespace Token.BusinessLogic.RequestHandlers
{
    public class IntentRequestHandler : IRequestHandler, IIntentRequestHandler
    {
        private static readonly TextInfo TEXT_INFO = new CultureInfo("en-US", false).TextInfo;

        private ILogger<IntentRequestHandler> logger;

        public IntentRequestHandler(ILogger<IntentRequestHandler> logger)
        {
            if (logger is null)
            {
                throw new ArgumentNullException("logger");
            }

            this.logger = logger;
        }

        public async Task<SkillResponse> GetSkillResponse(SkillRequest skillRequest, TokenUser tokenUser)
        {
            this.logger.LogTrace("BEGIN GetSkillResponse. RequestId: {0}.", skillRequest.Request.RequestId);

            if (skillRequest is null)
            {
                throw new ArgumentNullException("skillRequest");
            }

            if (tokenUser is null)
            {
                throw new ArgumentNullException("tokenUser");
            }

            IntentRequest intentRequest = skillRequest.Request as IntentRequest;

            if (intentRequest.Intent.ConfirmationStatus == "DENIED")
            {
                return string.Format("Okay").Tell();
            }

            SkillResponse speechResponse = null;

            switch (intentRequest.Intent.Name)
            {
                case IntentRequestName.AddPoints:
                    speechResponse = await Task.Run(() => this.AddPoints(skillRequest, tokenUser));
                    break;
                case IntentRequestName.AddPlayer:
                    speechResponse = await Task.Run(() => this.AddPlayer(skillRequest, tokenUser));
                    break;
                case IntentRequestName.DeletePlayer:
                    speechResponse = await Task.Run(() => this.DeletePlayer(skillRequest, tokenUser));
                    break;
                case IntentRequestName.RemovePoints:
                    speechResponse = await Task.Run(() => this.RemovePoints(skillRequest, tokenUser));
                    break;
                case IntentRequestName.DeleteAllPlayers:
                    speechResponse = await Task.Run(() => this.DeleteAllPlayers(skillRequest, tokenUser));
                    break;
                case IntentRequestName.ListAllPlayers:
                    speechResponse = await Task.Run(() => this.ListAllPlayers(skillRequest, tokenUser));
                    break;
                case IntentRequestName.GetPlayerPoints:
                    speechResponse = await Task.Run(() => this.GetPlayerPoints(skillRequest, tokenUser));
                    break;
                case IntentRequestName.ResetAllPoints:
                    speechResponse = await Task.Run(() => this.ResetAllPoints(skillRequest, tokenUser));
                    break;
                case IntentRequestName.GetPointsMax:
                    speechResponse = await Task.Run(() => this.GetPointsMax(skillRequest, tokenUser));
                    break;
                case IntentRequestName.GetPointsMin:
                    speechResponse = await Task.Run(() => this.GetPointsMin(skillRequest, tokenUser));
                    break;
                case IntentRequestName.GetPointsAverage:
                    speechResponse = await Task.Run(() => this.GetPointsAverage(skillRequest, tokenUser));
                    break;
                case IntentRequestName.ListAllPoints:
                    speechResponse = await Task.Run(() => this.ListAllPoints(skillRequest, tokenUser));
                    break;
                case IntentRequestName.GetAllPlayersCount:
                    speechResponse = await Task.Run(() => this.GetAllPlayersCount(skillRequest, tokenUser));
                    break;
                case IntentRequestName.AddAllPoints:
                    speechResponse = await Task.Run(() => this.AddAllPoints(skillRequest, tokenUser));
                    break;
                case IntentRequestName.AddSinglePoint:
                    speechResponse = await Task.Run(() => this.AddSinglePoint(skillRequest, tokenUser));
                    break;
                case IntentRequestName.RemoveSinglePoint:
                    speechResponse = await Task.Run(() => this.RemoveSinglePoint(skillRequest, tokenUser));
                    break;
                case IntentRequestName.RemoveAllPoints:
                    speechResponse = await Task.Run(() => this.RemoveAllPoints(skillRequest, tokenUser));
                    break;
                default:
                    break;
            }

            this.logger.LogTrace("END GetSkillResponse. RequestId: {0}.", skillRequest.Request.RequestId);

            return speechResponse;
        }

        public SkillResponse AddPlayer(SkillRequest skillRequest, TokenUser tokenUser)
        {
            this.logger.LogTrace("BEGIN AddPlayer. RequestId: {0}.", skillRequest.Request.RequestId);

            IntentRequest intentRequest = skillRequest.Request as IntentRequest;

            string playerName = IntentRequestHandler.TEXT_INFO.ToTitleCase(intentRequest.Intent.Slots["player"].Value);

            Player existingPlayer = tokenUser.Players.FirstOrDefault(x => x.Name == playerName);

            SkillResponse response;
            if (existingPlayer != null)
            {
                // Don't update any data
                response = string.Format("{0} is already in your list of players.", existingPlayer.Name).Tell();
            }
            else
            {
                // Add new Player data
                tokenUser.Players.Add(new Player() { Name = playerName });
                response = string.Format("Alright, I added {0} to your list of players.", playerName).Tell();
            }

            this.logger.LogTrace("END AddPlayer. RequestId: {0}.", skillRequest.Request.RequestId);

            return response;
        }

        public SkillResponse AddPoints(SkillRequest skillRequest, TokenUser tokenUser)
        {
            this.logger.LogTrace("BEGIN AddPoints. RequestId: {0}.", skillRequest.Request.RequestId);

            IntentRequest intentRequest = skillRequest.Request as IntentRequest;

            string playerName = IntentRequestHandler.TEXT_INFO.ToTitleCase(intentRequest.Intent.Slots["player"].Value);
            int points = Int32.Parse(intentRequest.Intent.Slots["amount"].Value);

            Player existingPlayer = tokenUser.Players.FirstOrDefault(x => x.Name == playerName);

            string pointsResponseWord = points != Math.Abs(1) ? "points" : "point";

            SkillResponse response = null;
            if (existingPlayer != null)
            {
                // Remove old Player data
                tokenUser.Players = tokenUser.Players.Where(x => x.Name != existingPlayer.Name).ToList();

                // Add updated Player data
                existingPlayer.Points += points;
                tokenUser.Players.Add(existingPlayer);

                response = string.Format("Okay, I added {0} {1} to {2}.", points, pointsResponseWord, existingPlayer.Name).Tell();
            }
            else
            {
                // Add new Player data
                tokenUser.Players.Add(new Player() { Name = playerName, Points = points });
                response = string.Format("Alright, I added {0} to your list of players and gave them {1} {2}.", playerName, points, pointsResponseWord).Tell();
            }

            this.logger.LogTrace("END AddPoints. RequestId: {0}.", skillRequest.Request.RequestId);

            return response;
        }

        public SkillResponse DeletePlayer(SkillRequest skillRequest, TokenUser tokenUser)
        {
            this.logger.LogTrace("BEGIN DeletePlayer. RequestId: {0}.", skillRequest.Request.RequestId);

            IntentRequest intentRequest = skillRequest.Request as IntentRequest;

            string playerName = IntentRequestHandler.TEXT_INFO.ToTitleCase(intentRequest.Intent.Slots["player"].Value);

            Player existingPlayer = tokenUser.Players.FirstOrDefault(x => x.Name == playerName);

            StringBuilder responsePhraseBuilder = new StringBuilder();

            SkillResponse response = null;
            if (existingPlayer == null)
            {
                response = string.Format("Hmm, I don't see {0} in your list of players.", playerName).Tell();
            }
            else
            {
                // Remove Player from list
                tokenUser.Players = tokenUser.Players.Where(x => x.Name != existingPlayer.Name).ToList();

                response = string.Format("Okay, I removed {0} from your list of players.", playerName).Tell();
            }

            this.logger.LogTrace("END DeletePlayer. RequestId: {0}.", skillRequest.Request.RequestId);

            return response;
        }

        public SkillResponse RemovePoints(SkillRequest skillRequest, TokenUser tokenUser)
        {
            this.logger.LogTrace("BEGIN RemovePoints. RequestId: {0}.", skillRequest.Request.RequestId);

            IntentRequest intentRequest = skillRequest.Request as IntentRequest;

            string playerName = IntentRequestHandler.TEXT_INFO.ToTitleCase(intentRequest.Intent.Slots["player"].Value);
            int points = Int32.Parse(intentRequest.Intent.Slots["amount"].Value);

            Player existingPlayer = tokenUser.Players.FirstOrDefault(x => x.Name == playerName);

            SkillResponse response = null;
            if (existingPlayer != null)
            {
                // Remove old Player data
                tokenUser.Players = tokenUser.Players.Where(x => x.Name != existingPlayer.Name).ToList();

                // Add updated Player data, remove points
                existingPlayer.Points -= points;
                tokenUser.Players.Add(existingPlayer);

                string pointsResponseWord = points != Math.Abs(1) ? "points" : "point";
                response = string.Format("Okay, I removed {0} {1} from {2}.", points, pointsResponseWord, existingPlayer.Name).Tell();
            }
            else
            {
                response = string.Format("Hmm, I don't see {0} in your list of players.", playerName).Tell();
            }

            this.logger.LogTrace("END RemovePoints. RequestId: {0}.", skillRequest.Request.RequestId);

            return response;
        }

        public SkillResponse ListAllPoints(SkillRequest skillRequest, TokenUser tokenUser)
        {
            this.logger.LogTrace("BEGIN ListAllPoints. RequestId: {0}.", skillRequest.Request.RequestId);

            SkillResponse response = null;
            if (tokenUser.Players == null || tokenUser.Players.Count <= 0)
            {
                response = string.Format("Hmm, you don't have any players yet.").Tell();
            }
            else
            {
                StringBuilder responsePhraseBuilder = new StringBuilder();
                responsePhraseBuilder.Append("Okay, here we go. From highest to lowest.");

                tokenUser.Players.OrderByDescending(x => x.Points).ToList().ForEach(x =>
                {
                    string pointsWord = Math.Abs(x.Points) != 1 ? "points" : "point";
                    responsePhraseBuilder.AppendFormat(" {0} has {1} {2}.", x.Name, x.Points, pointsWord);
                });

                responsePhraseBuilder.AppendFormat(" I think that's everybody.");

                response = responsePhraseBuilder.ToString().Tell();
            }

            this.logger.LogTrace("END ListAllPoints. RequestId: {0}.", skillRequest.Request.RequestId);

            return response;
        }

        public SkillResponse ListAllPlayers(SkillRequest skillRequest, TokenUser tokenUser)
        {
            this.logger.LogTrace("BEGIN ListAllPlayers. RequestId: {0}.", skillRequest.Request.RequestId);

            SkillResponse response = null;
            if (tokenUser.Players == null || tokenUser.Players.Count <= 0)
            {
                response = string.Format("Hmm, you don't have any players yet.").Tell();
            }
            else
            {
                StringBuilder responsePhraseBuilder = new StringBuilder();

                responsePhraseBuilder.Append("Okay, here we go. The players in your list are");

                string[] arrayOfPlayersNames = tokenUser.Players.Select(x => x.Name).ToArray();
                for (int i = 0; i < arrayOfPlayersNames.Length; i++)
                {
                    if (i == 0)
                    {
                        responsePhraseBuilder.AppendFormat(" {0}", arrayOfPlayersNames[i]);
                        continue;
                    }

                    if (i == arrayOfPlayersNames.Length - 1)
                    {
                        responsePhraseBuilder.AppendFormat(" and {0}.", arrayOfPlayersNames[i]);
                        continue;
                    }

                    responsePhraseBuilder.AppendFormat(", {0}", arrayOfPlayersNames[i]);
                }

                responsePhraseBuilder.Append(" I think that's everybody.");

                response = responsePhraseBuilder.ToString().Tell();
            }

            this.logger.LogTrace("END ListAllPlayers. RequestId: {0}.", skillRequest.Request.RequestId);

            return response;
        }

        public SkillResponse GetPlayerPoints(SkillRequest skillRequest, TokenUser tokenUser)
        {
            this.logger.LogTrace("BEGIN GetPlayerPoints. RequestId: {0}.", skillRequest.Request.RequestId);

            IntentRequest intentRequest = skillRequest.Request as IntentRequest;

            string playerName = IntentRequestHandler.TEXT_INFO.ToTitleCase(intentRequest.Intent.Slots["player"].Value);

            Player existingPlayer = tokenUser.Players.FirstOrDefault(x => x.Name == playerName);

            SkillResponse response;
            if (existingPlayer != null)
            {
                string pointsWord = Math.Abs(existingPlayer.Points) != 1 ? "points" : "point";
                response = string.Format("{0} has {1} {2}.", existingPlayer.Name, existingPlayer.Points, pointsWord).Tell();
            }
            else
            {
                response = string.Format("Hmm, I don't see {0} in your list of players.", playerName).Tell();
            }

            this.logger.LogTrace("END GetPlayerPoints. RequestId: {0}.", skillRequest.Request.RequestId);

            return response;
        }

        public SkillResponse GetPointsAverage(SkillRequest skillRequest, TokenUser tokenUser)
        {
            this.logger.LogTrace("BEGIN GetPointsAverage. RequestId: {0}.", skillRequest.Request.RequestId);

            IntentRequest intentRequest = skillRequest.Request as IntentRequest;


            double[] allPoints = tokenUser.Players.Select(x => (double)x.Points).ToArray();
            double averagePoints = allPoints.Average();

            string pointsWord = Math.Abs(averagePoints) != 1 ? "points" : "point";
            SkillResponse response = string.Format("The average score for all players is {0} {1}.", averagePoints, pointsWord).Tell();

            this.logger.LogTrace("END GetPointsAverage. RequestId: {0}.", skillRequest.Request.RequestId);

            return response;
        }

        public SkillResponse GetPointsMax(SkillRequest skillRequest, TokenUser tokenUser)
        {
            this.logger.LogTrace("BEGIN GetPointsMax. RequestId: {0}.", skillRequest.Request.RequestId);

            IntentRequest intentRequest = skillRequest.Request as IntentRequest;

            Player[] playersScoreDescending = tokenUser.Players.OrderByDescending(x => x.Points).ToArray();

            SkillResponse response;

            if (playersScoreDescending == null)
            {
                response = string.Format("Hmm, you don't see anyone in your list of players.").Tell();
            }
            else
            {
                int highScore = playersScoreDescending[0].Points;
                Player[] highScorePlayers = playersScoreDescending.Where(x => x.Points == highScore).ToArray();

                if (highScorePlayers.Length == playersScoreDescending.Count())
                {
                    string pointsWord = Math.Abs(highScore) != 1 ? "points" : "point";
                    response = string.Format("All players are tied with a score of {0} {1}.", highScore, pointsWord).Tell();
                }
                else if (highScorePlayers.Length > 1)
                {
                    StringBuilder responsePhraseBuilder = new StringBuilder();

                    for (int i = 0; i < highScorePlayers.Count(); i++)
                    {
                        Player currentPlayer = highScorePlayers[i];
                        if (i == 0)
                        {
                            responsePhraseBuilder.AppendFormat("{0}", currentPlayer.Name);
                            continue;
                        }

                        responsePhraseBuilder.AppendFormat("and {0}", currentPlayer.Name);
                    }

                    string pointsWord = Math.Abs(highScore) != 1 ? "points" : "point";
                    responsePhraseBuilder.AppendFormat(" are tied for the high score with {0} {1}.", highScore, pointsWord);

                    response = responsePhraseBuilder.ToString().Tell();
                }
                else
                {
                    string pointsWord = Math.Abs(highScore) != 1 ? "points" : "point";
                    response = string.Format("{0} has the highest score with {1} {2}.", highScorePlayers[0].Name, highScore, pointsWord).Tell();
                }
            }

            this.logger.LogTrace("END GetPointsMax. RequestId: {0}.", skillRequest.Request.RequestId);

            return response;
        }

        public SkillResponse GetPointsMin(SkillRequest skillRequest, TokenUser tokenUser)
        {
            this.logger.LogTrace("BEGIN GetPointsMin. RequestId: {0}.", skillRequest.Request.RequestId);

            IntentRequest intentRequest = skillRequest.Request as IntentRequest;

            Player[] playersScoreAscending = tokenUser.Players.OrderBy(x => x.Points).ToArray();

            SkillResponse response;

            if (playersScoreAscending == null)
            {
                response = string.Format("Hmm, you don't see anyone in your list of players.").Tell();
            }
            else
            {
                int lowScore = playersScoreAscending[0].Points;
                Player[] lowScorePlayers = playersScoreAscending.Where(x => x.Points == lowScore).ToArray();

                if (lowScorePlayers.Length == playersScoreAscending.Count())
                {
                    string pointsWord = Math.Abs(lowScore) != 1 ? "points" : "point";
                    response = string.Format("All players are tied with a score of {0} {1}.", lowScore, pointsWord).Tell();
                }
                else if (lowScorePlayers.Length > 1)
                {
                    StringBuilder responsePhraseBuilder = new StringBuilder();

                    for (int i = 0; i < lowScorePlayers.Count(); i++)
                    {
                        Player currentPlayer = lowScorePlayers[i];
                        if (i == 0)
                        {
                            responsePhraseBuilder.AppendFormat("{0}", currentPlayer.Name);
                            continue;
                        }

                        responsePhraseBuilder.AppendFormat("and {0}", currentPlayer.Name);
                    }

                    string pointsWord = Math.Abs(lowScore) != 1 ? "points" : "point";
                    responsePhraseBuilder.AppendFormat(" are tied for the lowest score with {0} {1}.", lowScore, pointsWord);

                    response = responsePhraseBuilder.ToString().Tell();
                }
                else
                {
                    string pointsWord = Math.Abs(lowScore) != 1 ? "points" : "point";
                    response = string.Format("{0} has the lowest score with {1} {2}.", lowScorePlayers[0].Name, lowScore, pointsWord).Tell();
                }
            }

            this.logger.LogTrace("END GetPointsMin. RequestId: {0}.", skillRequest.Request.RequestId);

            return response;
        }

        public SkillResponse DeleteAllPlayers(SkillRequest skillRequest, TokenUser tokenUser)
        {
            this.logger.LogTrace("BEGIN DeleteAllPlayers. RequestId: {0}.", skillRequest.Request.RequestId);

            SkillResponse response;

            tokenUser.Players = new List<Player>();

            response = string.Format("Alright, I removed everyone from your list of players.").Tell();

            this.logger.LogTrace("END DeleteAllPlayers. RequestId: {0}.", skillRequest.Request.RequestId);

            return response;
        }

        public SkillResponse ResetAllPoints(SkillRequest skillRequest, TokenUser tokenUser)
        {
            this.logger.LogTrace("BEGIN ResetAllPoints. RequestId: {0}.", skillRequest.Request.RequestId);

            SkillResponse response;

            tokenUser.Players = tokenUser.Players.Select(x => new Player() { Name = x.Name, Points = 0 }).ToList();

            response = string.Format("Okay, I reset all of the players' points to zero.").Tell();

            this.logger.LogTrace("END ResetAllPoints. RequestId: {0}.", skillRequest.Request.RequestId);

            return response;
        }

        public SkillResponse GetAllPlayersCount(SkillRequest skillRequest, TokenUser tokenUser)
        {
            this.logger.LogTrace("BEGIN GetAllPlayersCount. RequestId: {0}.", skillRequest.Request.RequestId);

            SkillResponse response;
            if (tokenUser.Players == null)
            {
                response = string.Format("There are no players your in your list.").Tell();
            }
            else
            {
                response = string.Format("There are {0} players your in your list.", tokenUser.Players.Count).Tell();
            }

            this.logger.LogTrace("END GetAllPlayersCount. RequestId: {0}.", skillRequest.Request.RequestId);

            return response;
        }

        public SkillResponse AddAllPoints(SkillRequest skillRequest, TokenUser tokenUser)
        {
            this.logger.LogTrace("BEGIN AddAllPoints. RequestId: {0}.", skillRequest.Request.RequestId);

            IntentRequest intentRequest = skillRequest.Request as IntentRequest;

            int points = Int32.Parse(intentRequest.Intent.Slots["amount"].Value);

            SkillResponse response = null;
            if (tokenUser.Players == null || tokenUser.Players.Count <= 0)
            {
                response = string.Format("Hmm, I don't see anyone in your list of players.").Tell();
            }
            else
            {
                tokenUser.Players = tokenUser.Players.Select(x => new Player() { Name = x.Name, Points = x.Points + points }).ToList();

                string pointsResponseWord = points != Math.Abs(1) ? "points" : "point";
                response = string.Format("Okay, I gave all players {0} {1}.", points, pointsResponseWord).Tell();
            }

            this.logger.LogTrace("END AddAllPoints. RequestId: {0}.", skillRequest.Request.RequestId);

            return response;
        }

        public SkillResponse RemoveAllPoints(SkillRequest skillRequest, TokenUser tokenUser)
        {
            this.logger.LogTrace("BEGIN RemoveAllPoints. RequestId: {0}.", skillRequest.Request.RequestId);

            IntentRequest intentRequest = skillRequest.Request as IntentRequest;

            int points = Int32.Parse(intentRequest.Intent.Slots["amount"].Value);

            SkillResponse response = null;
            if (tokenUser.Players == null || tokenUser.Players.Count <= 0)
            {
                response = string.Format("Hmm, I don't see anyone in your list of players.").Tell();
            }
            else
            {
                tokenUser.Players = tokenUser.Players.Select(x => new Player() { Name = x.Name, Points = x.Points - points }).ToList(); ;

                string pointsResponseWord = points != Math.Abs(1) ? "points" : "point";
                response = string.Format("Okay, I removed {0} {1} from all players.", points, pointsResponseWord).Tell();
            }

            this.logger.LogTrace("END RemoveAllPoints. RequestId: {0}.", skillRequest.Request.RequestId);

            return response;
        }

        public SkillResponse AddSinglePoint(SkillRequest skillRequest, TokenUser tokenUser)
        {
            this.logger.LogTrace("BEGIN AddSinglePoint. RequestId: {0}.", skillRequest.Request.RequestId);

            IntentRequest intentRequest = skillRequest.Request as IntentRequest;

            string playerName = IntentRequestHandler.TEXT_INFO.ToTitleCase(intentRequest.Intent.Slots["player"].Value);

            Player existingPlayer = tokenUser.Players.FirstOrDefault(x => x.Name == playerName);

            SkillResponse response = null;
            if (existingPlayer != null)
            {
                // Remove old Player data
                tokenUser.Players = tokenUser.Players.Where(x => x.Name != existingPlayer.Name).ToList();

                // Add updated Player data
                existingPlayer.Points += 1;
                tokenUser.Players.Add(existingPlayer);

                response = string.Format("Okay, I added one point to {0}.", existingPlayer.Name).Tell();
            }
            else
            {
                // Add new Player data
                tokenUser.Players.Add(new Player() { Name = playerName, Points = 1 });
                response = string.Format("Alright, I added {0} to your list of players and gave them one point.", playerName).Tell();
            }

            this.logger.LogTrace("END AddSinglePoint. RequestId: {0}.", skillRequest.Request.RequestId);

            return response;
        }

        public SkillResponse RemoveSinglePoint(SkillRequest skillRequest, TokenUser tokenUser)
        {
            this.logger.LogTrace("BEGIN RemoveSinglePoint. RequestId: {0}.", skillRequest.Request.RequestId);

            IntentRequest intentRequest = skillRequest.Request as IntentRequest;

            string playerName = IntentRequestHandler.TEXT_INFO.ToTitleCase(intentRequest.Intent.Slots["player"].Value);

            Player existingPlayer = tokenUser.Players.FirstOrDefault(x => x.Name == playerName);

            SkillResponse response = null;
            if (existingPlayer != null)
            {
                // Remove old Player data
                tokenUser.Players = tokenUser.Players.Where(x => x.Name != existingPlayer.Name).ToList();

                // Add updated Player data
                existingPlayer.Points -= 1;
                tokenUser.Players.Add(existingPlayer);

                response = string.Format("Okay, I removed one point from {0}.", existingPlayer.Name).Tell();
            }
            else
            {
                response = string.Format("Hmm, I don't see {0} in your list of players.", playerName).Tell();
            }

            this.logger.LogTrace("END RemoveSinglePoint. RequestId: {0}.", skillRequest.Request.RequestId);

            return response;
        }
    }
}