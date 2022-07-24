using Repository.Model;
using System.Threading.Tasks;
using TestUtilities.Extensions;

namespace CustomerAPI.Tests.Common
{
    public static class CustomerSteps
    {
        public static Feature WhenIGetAllCustomers(this Feature feature)
        {
            Task.Run(async () => feature.Response = await feature.Client.GetAsync($"/api/Clients/clients")).Wait();

            return feature;
        }

        public static Feature WhenIGetCustomerWithUsername(this Feature feature, string userName)
        {
            Task.Run(async () => feature.Response = await feature.Client.GetAsync($"/api/Clients/getclientbyid/{userName}")).Wait();

            return feature;
        }

        public static Feature WhenIAddCustomer(this Feature feature, ClientMasterModel model)
        {
            Task.Run(async () => feature.Response = await feature.Client.PostAsync($"/api/Clients/add", model.AsJson())).Wait();

            return feature;
        }

        public static Feature WhenIUpdateACustomer(this Feature feature, ClientMasterModel model)
        {
            Task.Run(async () => feature.Response = await feature.Client.PostAsync($"/api/Clients/update", model.AsJson())).Wait();

            return feature;
        }

        public static Feature WhenIRegisterACustomer(this Feature feature, ClientMasterModel model)
        {
            Task.Run(async () => feature.Response = await feature.Client.PostAsync($"/api/Clients/register", model.AsJson())).Wait();

            return feature;
        }

        public static Feature WhenIAuthenticate(this Feature feature, LoginRequestModel model)
        {
            Task.Run(async () => feature.Response = await feature.Client.PostAsync($"/api/Clients/authenticate", model.AsJson())).Wait();

            return feature;
        }
    }
}
