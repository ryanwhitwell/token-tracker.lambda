
using Microsoft.Extensions.DependencyInjection;
using Token.Data;
using Token.Data.Interfaces;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Token.BusinessLogic;
using NLog.Config;
using NLog;
using Token.BusinessLogic.Interfaces;
using Amazon.DynamoDBv2.DataModel;
using Amazon;
using Amazon.DynamoDBv2;
using Token.BusinessLogic.IntentRequestHandlers;
using Token.BusinessLogic.LaunchRequestHandlers;
using Token.BusinessLogic.ConnectionResponseRequestHandlers;

namespace Token.Core
{
  public static class IOC
  {
    public static readonly ServiceProvider Container = GetServiceProvider();

    private static ServiceProvider GetServiceProvider()
    {
      IServiceCollection serviceCollection = new ServiceCollection();

      // Logging
      serviceCollection.AddLogging(loggingBuilder =>
        {
          loggingBuilder.ClearProviders();
          loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
          loggingBuilder.AddNLog();
        });

      // Business Logic
      serviceCollection.AddSingleton<IDynamoDBContext>(new DynamoDBContext(new AmazonDynamoDBClient(RegionEndpoint.USEast1), new DynamoDBContextConfig() { ConsistentRead = true }))
                        .AddTransient<IRequestBusinessLogic,       RequestBusinessLogic>()
                        .AddTransient<IRequestMapper,              RequestMapper>()
                        .AddTransient<ITokenUserData,              TokenUserData>()
                        .AddTransient<ITokenUserRepository,        TokenUserRepository>()
                        .AddTransient<ISkillProductsClientAdapter, SkillProductsClientAdapter>()
                        .AddTransient<ISkillRequestValidator,      SkillRequestValidator>();

      // SessionEndedRequest
      serviceCollection.AddTransient<IRequestRouter,              SessionEndedRequestRouter>()
                       .AddTransient<ISessionEndedRequestHandler, DefaultSessionEndedRequest>();

      // ConnectionResponseRequest
      serviceCollection.AddTransient<IRequestRouter,                    ConnectionResponseRequestRouter>()
                       .AddTransient<IConnectionResponseRequestHandler, DefaultConnectionResponseRequest>();
        
      // LaunchRequest
      serviceCollection.AddTransient<IRequestRouter,        LaunchRequestRouter>()
                       .AddTransient<ILaunchRequestHandler, DefaultLaunchRequest>();
        
      // IntentRequest  
      serviceCollection.AddTransient<IRequestRouter,        IntentRequestRouter>()
                       .AddTransient<IIntentRequestHandler, AddAllPoints>()
                       .AddTransient<IIntentRequestHandler, AddPlayer>()
                       .AddTransient<IIntentRequestHandler, AddPoints>()
                       .AddTransient<IIntentRequestHandler, AddSinglePoint>()
                       .AddTransient<IIntentRequestHandler, DeleteAllPlayers>()
                       .AddTransient<IIntentRequestHandler, DeletePlayer>()
                       .AddTransient<IIntentRequestHandler, GetAllPlayersCount>()
                       .AddTransient<IIntentRequestHandler, GetPlayerPoints>()
                       .AddTransient<IIntentRequestHandler, GetPointsAverage>()
                       .AddTransient<IIntentRequestHandler, GetPointsMax>()
                       .AddTransient<IIntentRequestHandler, GetPointsMin>()
                       .AddTransient<IIntentRequestHandler, ListAllPlayers>()
                       .AddTransient<IIntentRequestHandler, ListAllPoints>()
                       .AddTransient<IIntentRequestHandler, RemoveAllPoints>()
                       .AddTransient<IIntentRequestHandler, RemovePoints>()
                       .AddTransient<IIntentRequestHandler, RemoveSinglePoint>()
                       .AddTransient<IIntentRequestHandler, ResetAllPoints>()
                       .AddTransient<IIntentRequestHandler, Buy>()
                       .AddTransient<IIntentRequestHandler, WhatCanIBuy>()
                       .AddTransient<IIntentRequestHandler, Help>()
                       .AddTransient<IIntentRequestHandler, RefundSubscription>()
                       .AddTransient<IIntentRequestHandler, Fallback>()
                       .AddTransient<IIntentRequestHandler, Stop>()
                       .AddTransient<IIntentRequestHandler, Cancel>()
                       .AddTransient<IIntentRequestHandler, NavigateHome>();

      // Localization
      serviceCollection.AddLocalization(x => x.ResourcesPath = "Resources");
          
      // Set logging configuration
      LoggingConfiguration nlogConfig = new NLogLoggingConfiguration(Configuration.File.GetSection("NLog"));
      LogManager.Configuration = nlogConfig;

      return serviceCollection.BuildServiceProvider();;
    }
  }
}