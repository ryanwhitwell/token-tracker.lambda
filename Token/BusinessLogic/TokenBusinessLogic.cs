using System;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;
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
            SsmlOutputSpeech speech = new SsmlOutputSpeech();
            speech.Ssml = string.Format("<speak>Hey {0}, this app is really awesome.</speak>", "Sara");

            SkillResponse speechResponse = await Task.Run(() => ResponseBuilder.Tell(speech));

            return speechResponse;
        }
    }
}