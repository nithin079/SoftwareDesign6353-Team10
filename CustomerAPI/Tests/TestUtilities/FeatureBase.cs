using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace TestUtilities
{
    public abstract class FeatureBase<TFeature> : IDisposable where TFeature : class
    {
        public IConfiguration Configuration { get; private set; }
        protected IServiceProvider ServiceProvider { get; private set; }
        protected virtual Action<IServiceCollection> AdditionalServices { get; } = null;
        public virtual TFeature Initialize(
            Action<IConfiguration> additionalConfiguration = null,
            Action<IServiceCollection> additionalServiceConfiguration = null)
        {
            Configuration = TestConfig.GetConfiguration();

            additionalConfiguration?.Invoke(Configuration);

            var serviceCollection = new ServiceCollection();
            ConfigureServices?.Invoke(serviceCollection);
            additionalServiceConfiguration?.Invoke(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            return this as TFeature;
        }

        protected virtual Action<IServiceCollection> ConfigureServices
            => services =>
            {
                //add test database tables if necessary
                AdditionalServices?.Invoke(services);
            };

        public virtual void Dispose() { }
    }
}
