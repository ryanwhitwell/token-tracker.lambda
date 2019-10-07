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
using Newtonsoft.Json;
using System.Globalization;

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

    private void SetCulture(SkillRequest skillRequest)
    {
      if (skillRequest.Request == null)
      {
        throw new ArgumentNullException("request");
      }

      if (string.IsNullOrWhiteSpace(skillRequest.Request.Locale))
      {
        throw new ArgumentNullException("locale");
      }

      CultureInfo.CurrentCulture = new CultureInfo(skillRequest.Request.Locale);
      CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture;
    }

    public async Task<SkillResponse> FunctionHandler(SkillRequest skillRequest, ILambdaContext context)
    {
      // Skill ID verified by AWS Lambda service configuration
      if (skillRequest.Version == "WARMING")
      {
        return null;
      }

      Logger logger = LogManager.GetCurrentClassLogger();

      SkillResponse response;
      try
      {
        // Set current culture
        SetCulture(skillRequest);

        logger.Log(LogLevel.Debug, "SkillRequest: " + JsonConvert.SerializeObject(skillRequest));
        
        if (skillRequest.Context.System.User == null ||
            string.IsNullOrWhiteSpace(skillRequest.Context.System.User.AccessToken))
        {
          // Send user a message with an Account Linking card
          response = string.Format("Before I can keep track of your tokens, you need to log in with your Amazon account. Please visit the home screen on the Alexa app to link your Amazon account.").TellWithCard(new LinkAccountCard());

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
