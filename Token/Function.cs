using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using System.Threading.Tasks;
using Token.Core;
using NLog;
using Token.BusinessLogic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace Token
{
    public class Function
    {
        // Initialize Configuration
        private static IConfigurationRoot configurationFile = Configuration.File;
        // Initialize DI Container
        private static ServiceProvider container = IOC.Container;
        private ITokenBusinessLogic businessLogic = container.GetService<ITokenBusinessLogic>();
        
        public async Task<SkillResponse> FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            SkillResponse response = await this.businessLogic.HandleSkillRequest(input, context);

            LogManager.GetCurrentClassLogger().Log(NLog.LogLevel.Error, "This is an ERROR. Please help!");
            
            return response;
        }
    }
}
