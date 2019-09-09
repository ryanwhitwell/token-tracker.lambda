
using Microsoft.Extensions.DependencyInjection;
using Token.DataAccess;
using Token.DataAccess.Interfaces;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Token.BusinessLogic;
using NLog.Config;
using NLog;

namespace Token.Core
{
    public static class IOC
    {
        public static readonly ServiceProvider Container =  GetServiceProvider();

        private static ServiceProvider GetServiceProvider()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddLogging(loggingBuilder =>
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                    loggingBuilder.AddNLog();
                })
                .AddTransient(typeof(ITokenBusinessLogic), typeof(TokenBusinessLogic))
                .AddTransient(typeof(IUserRepository), typeof(UserRepository))
                .BuildServiceProvider();

            LoggingConfiguration nlogConfig = new NLogLoggingConfiguration(Configuration.File.GetSection("NLog"));
            LogManager.Configuration = nlogConfig;

            return serviceProvider;
        }
    }
}