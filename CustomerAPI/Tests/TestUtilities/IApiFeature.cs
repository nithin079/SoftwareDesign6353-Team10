using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace TestUtilities
{
    public interface IApiFeature<TFeature> where TFeature : class
    {
        public HttpResponseMessage Response { get; set; }
        public HttpClient Client { get; set; }

        void DeleteAllTableData();
    }
}
