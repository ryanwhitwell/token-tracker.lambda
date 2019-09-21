
using Microsoft.Extensions.DependencyInjection;
using Token.DataAccess;
using Token.DataAccess.Interfaces;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Token.BusinessLogic;
using NLog.Config;
using NLog;
using Token.BusinessLogic.Interfaces;
using Amazon.DynamoDBv2.DataModel;
using Amazon;
using Amazon.DynamoDBv2;

namespace Token.Core
{
  public static class IOC
  {
    public static readonly ServiceProvider Container = GetServiceProvider();

    private static ServiceProvider GetServiceProvider()
    {
      ServiceProvider serviceProvider = new ServiceCollection()
          .AddLogging(loggingBuilder =>
          {
            loggingBuilder.ClearProviders();
            loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
            loggingBuilder.AddNLog();
          })
          .AddTransient<IRequestBusinessLogic, RequestBusinessLogic>()
          .AddTransient<IRequestRouter, IntentRequestRouter>()
          .AddTransient<ITokenUserData, TokenUserData>()
          .AddTransient<ITokenUserRepository, TokenUserRepository>()
          .AddSingleton<IDynamoDBContext>(new DynamoDBContext(new AmazonDynamoDBClient(RegionEndpoint.USEast1), new DynamoDBContextConfig() { ConsistentRead = true }))
          .BuildServiceProvider();

      LoggingConfiguration nlogConfig = new NLogLoggingConfiguration(Configuration.File.GetSection("NLog"));
      LogManager.Configuration = nlogConfig;

      return serviceProvider;
    }
  }
}