using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TestUtilities;
using TestUtilities.Framework;

namespace CustomerAPI.Tests
{
    public class Feature : FeatureBase<Feature>, IApiFeature<Feature>
    {
        public TestServer TestServer { get; set; }
        public HttpClient Client { get; set; }
        public HttpResponseMessage Response { get; set; }

        //Add Mock repositories that will be used per Feature here

        public Feature()
        {
            Initialize();
        }

        public override Feature Initialize(
            Action<IConfiguration> additionalConfiguration = null,
            Action<IServiceCollection> additionalServiceConfiguration = null)
        {
            base.Initialize(additionalConfiguration, services => ApplyAdditionalServiceConfiguration(services, additionalServiceConfiguration));

            try
            {
                TestServer = TestServerFactory.CreateTestServer(additionalConfiguration, services => ApplyAdditionalServiceConfiguration(services, additionalServiceConfiguration));

                Client = TestServer.CreateClient();
            }
            catch (Exception ex)
            {
                throw;
            }

            RegisterMockedTestServices();

            return this;
        }

        protected virtual void RegisterMockedTestServices()
        {

        }

        protected virtual void ApplyAdditionalServiceConfiguration(IServiceCollection services, Action<IServiceCollection> additionalServiceConfiguration)
        {
            AdditionalServices?.Invoke(services);
            additionalServiceConfiguration?.Invoke(services);
        }

        protected override Action<IServiceCollection> AdditionalServices
            => services =>
            {
                //services.AddSingleton<>();
            };

        public override void Dispose()
        {
            TestServer?.Dispose();

            DeleteAllTableData();

            base.Dispose();
        }

        public void DeleteAllTableData()
        {
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using var conn = new SqlConnection(connectionString);
            var command = conn.CreateCommand();
            command.CommandText = "DELETE FROM dbo.FuelQuoteMaster; DELETE FROM dbo.ClientsMaster; DELETE FROM dbo.PriceMaster; DELETE FROM dbo.RefreshToken";
            command.CommandType = CommandType.Text;

            conn.Open();
            command.ExecuteNonQuery();
        }
    }
	
}
