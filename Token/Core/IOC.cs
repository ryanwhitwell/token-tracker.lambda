
using Microsoft.Extensions.DependencyInjection;

namespace Token.Core
{
    public static class IOC
    {
        public static readonly ServiceProvider Container = InitializeServiceProvider();

        private static ServiceProvider InitializeServiceProvider()
        {
            ServiceCollection serviceCollection = new ServiceCollection();

            //// serviceCollection.AddScoped<IEmailSender, AuthMessageSender>();
            //// serviceCollection.AddScoped<AzureFunctionEventProcessor, IEventProcessor>();

            return serviceCollection.BuildServiceProvider();
        }
        
    }
}