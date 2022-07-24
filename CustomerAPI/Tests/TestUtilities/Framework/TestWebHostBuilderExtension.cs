using CustomerAPI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace TestUtilities.Framework
{
    public static class TestWebHostBuilderExtension
    {
        public static IWebHostBuilder ConfigureForTest(
            this IWebHostBuilder builder,
            Action<IConfiguration> additionalConfiguration = null,
            Action<IServiceCollection> additionalServiceConfiguration = null)
        {
            var configuration = TestConfig.GetConfiguration();
            additionalConfiguration?.Invoke(configuration);

            builder
                .UseTestServer()
                .UseConfiguration(configuration)
                .UseStartup<Startup>();

            builder.ConfigureTestServices(services => AddTestServices(services, configuration));

            if (additionalServiceConfiguration != null)
                builder.ConfigureTestServices(additionalServiceConfiguration);

            return builder;
        }

        public static void AddTestServices(IServiceCollection services, IConfiguration configuration)
        {
            //Mock Authentication here
            //Mock Logging here
            //Mock Test Database connection 
        }
    }
}
