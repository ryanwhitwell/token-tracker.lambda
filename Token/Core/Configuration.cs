using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Alexa.NET.InSkillPricing.Directives;
using Alexa.NET.InSkillPricing.Responses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NLog.Extensions.Logging;

namespace Token.Core
{
  public static class Configuration
  {
    private static string CONFIG_FILE_NAME = "appsettings.json";
    public static readonly IConfigurationRoot File = LoadConfigurationFile();

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