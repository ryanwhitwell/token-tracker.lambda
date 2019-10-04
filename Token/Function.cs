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
          return string.Format("To get the most out of this skill, please link it with your Amazon account.").TellWithCard(new LinkAccountCard());

        }
        
        response = await _businessLogic.HandleSkillRequest(skillRequest, context);
      }
      catch (Exception e)
      {
        logger.Log(LogLevel.Error, e);

        response = string.Format("I'm sorry, but I seem to be having trouble handling your request.").TellWithReprompt(string.Format(" If you need help you can say, ask {0} for help.", Configuration.File.GetSection("Application")["SkillName"]));
      }

      return response;
    }
  }
}
