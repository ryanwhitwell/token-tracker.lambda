using System.Globalization;
using System.IO;
using Alexa.NET.InSkillPricing.Directives;
using Alexa.NET.InSkillPricing.Responses;
using Microsoft.Extensions.Configuration;

namespace Token.Core
{
  public static class Configuration
  {
    private static string CONFIG_FILE_NAME = "appsettings.json";
    public static readonly IConfigurationRoot File = LoadConfigurationFile();
    public static readonly TextInfo TEXT_INFO = new CultureInfo("en-US", false).TextInfo;

    private static IConfigurationRoot LoadConfigurationFile()
    {
      IConfigurationBuilder builder = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile(Configuration.CONFIG_FILE_NAME, optional: false, reloadOnChange: false);

      IConfigurationRoot configurationRoot = builder.Build();

      PaymentDirective.AddSupport();
      ConnectionResponseHandler.AddToRequestConverter();

      return configurationRoot;
    }
  }
}