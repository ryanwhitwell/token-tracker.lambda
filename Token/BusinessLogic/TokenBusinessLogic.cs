using System;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;
using Microsoft.Extensions.Logging;
using Token.DataAccess.Interfaces;

namespace Token.BusinessLogic
{
    public class TokenBusinessLogic: ITokenBusinessLogic
    {
        private ILogger<TokenBusinessLogic> logger;
        private IUserRepository userRepository;
        public TokenBusinessLogic (ILogger<TokenBusinessLogic> logger, IUserRepository userRepository)
        {
            if (logger is null)
            {
                throw new ArgumentNullException("logger");
            }

            if (userRepository is null)
            {
                throw new ArgumentNullException("userRepository");
            }

            this.logger = logger;
            this.userRepository = userRepository;
        }

        public async Task<SkillResponse> HandleSkillRequest(SkillRequest input, ILambdaContext context)
        {
            // Test Logging in business logic
            this.logger.LogDebug("DI Container and logging are working in TokenBusinessLogic.");

            // Test Logging in business logic dependency
            await this.userRepository.Delete("abc123");

            // Ensure response functionality is working in business logic
            SsmlOutputSpeech speech = new SsmlOutputSpeech();
            speech.Ssml = string.Format("<speak>Hey {0}, this app is really awesome.</speak>", "Sara");

            SkillResponse speechResponse = ResponseBuilder.Tell(speech);

            string tableName = "User";
            //int itemsCount = await this.GetAllItemsCount(tableName);
            int itemsCount = 1;

            this.logger.LogInformation("Found {0} item(s) in the table '{1}'.", itemsCount, tableName);

            return speechResponse;
        }

        public async Task<int> GetAllItemsCount(string tableName)
        {
            AmazonDynamoDBClient client = new AmazonDynamoDBClient(RegionEndpoint.USEast1);
            ScanRequest req = new ScanRequest(tableName);
            ScanResponse resp = await client.ScanAsync(req);
            return resp.Count;
        }
    }
}