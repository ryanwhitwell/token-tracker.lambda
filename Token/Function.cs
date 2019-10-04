using Amazon.Lambda.Core;
using System.Threading.Tasks;
using Token.Core;
using NLog;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Token.BusinessLogic.Interfaces;
using Alexa.NET.InSkillPricing.Responses;
using Newtonsoft.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace Token
{
  public class Function
  {
    // Initialize Configuration
    private static readonly IConfigurationRoot configurationFile = Configuration.File;
    
    // Initialize DI Container
    private static readonly ServiceProvider container = IOC.Container;
    
    private IRequestBusinessLogic _businessLogic = container.GetService<IRequestBusinessLogic>();

    public async Task<SkillResponse> FunctionHandler(SkillRequest skillRequest, ILambdaContext context)
    {
      // Skill ID verified by AWS Lambda service
      Logger logger = LogManager.GetCurrentClassLogger();

      logger.Log(LogLevel.Debug, "SkillRequest: " + JsonConvert.SerializeObject(skillRequest));

      if (skillRequest.Version == "WARMING")
      {
        logger.Log(LogLevel.Info, "Keeping warm.");

        return null;
      }

      SkillResponse response;

      try
      {
        if (skillRequest.Context.System.User == null ||
            string.IsNullOrWhiteSpace(skillRequest.Context.System.User.AccessToken))
        {
          // Send user a message with an Account Linking card
          response = string.Format("Before I can keep track of your tokens, you need to log in with your Amazon account. Please visit the Alexa app to link your Amazon account.").TellWithCard(new LinkAccountCard());

          logger.Log(LogLevel.Debug, "SkillResponse: " + JsonConvert.SerializeObject(response));

          return response;
        
        }
        
        response = await _businessLogic.HandleSkillRequest(skillRequest, context);
      }
      catch (Exception e)
      {
        logger.Log(LogLevel.Error, e);

        response = string.Format("I'm sorry, but I seem to be having trouble handling your request.").TellWithReprompt(string.Format(" If you need help you can say, ask {0} for help.", Configuration.File.GetSection("Application")["SkillName"]));
      }

      logger.Log(LogLevel.Debug, "SkillResponse: " + JsonConvert.SerializeObject(response));

      return response;
    }
  }
}
