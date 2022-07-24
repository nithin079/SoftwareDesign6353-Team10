using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUtilities.Framework
{
    public static class TestServerFactory
    {
        public static TestServer CreateTestServer(Action<IConfiguration> additionalConfiguration, Action<IServiceCollection> additionalServiceConfiguration = null)
        {
            var testServer = new HostBuilder()
                .ConfigureWebHost(builder => builder.ConfigureForTest())
                .Start()
                .GetTestServer();

            return testServer;
        }
    }
}
