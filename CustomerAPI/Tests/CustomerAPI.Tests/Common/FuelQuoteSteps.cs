using Repository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtilities.Extensions;

namespace CustomerAPI.Tests.Common
{
    public static class FuelQuoteSteps
    {
        public static Feature WhenIGetHistoryOfFuelQuote(this Feature feature, FuelHistoryRequestModel model)
        {
            Task.Run(async () => feature.Response = await feature.Client.PostAsync($"/api/FuelQuote/getHistory", model.AsJson())).Wait();

            return feature;
        }

        public static Feature WhenIAddFuelQuoteForClient(this Feature feature, FuelQuoteMasterModel model)
        {
            Task.Run(async () => feature.Response = await feature.Client.PostAsync($"/api/FuelQuote/add", model.AsJson())).Wait();

            return feature;
        }
    }
}
