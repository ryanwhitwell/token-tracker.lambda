using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Token.Core
{
    public static class Configuration
    {
        private static string FILE_NAME = "appsettings.json";
        private static readonly IConfigurationRoot CONFIG_ROOT = InitializeConfiguration();

        private static IConfigurationRoot InitializeConfiguration()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(Configuration.FILE_NAME, optional: false, reloadOnChange: false);

            IConfigurationRoot configurationRoot = builder.Build();

            IEnumerable<IConfigurationSection> configSections = configurationRoot.GetChildren();

            IEnumerable<string> sectionNames = configSections.Select(x => x.Value);

            Console.WriteLine("Loaded configuration file with the following sections: {0}.", string.Join(", ", sectionNames));

            return configurationRoot;
        }

        public static IConfigurationSection Get(string section) 
        {
            return Configuration.CONFIG_ROOT.GetSection(section);
        }
    }
}