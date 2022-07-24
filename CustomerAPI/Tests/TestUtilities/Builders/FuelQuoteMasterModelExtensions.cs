using Repository.Model;
using TestUtilities.Data;

namespace TestUtilities.Builders
{
    public static class FuelQuoteMasterModelExtensions
    {
        public static FuelQuoteMasterModel WithDefaultValues(this FuelQuoteMasterModel model)
        {
            model.ClientId = TestData.ClientId;
            model.FuelId = TestData.FuelId;
            model.ClientName = TestData.Fullname;
            model.DiliveryAddress = TestData.Address1;

            return model;
        }

        public static FuelQuoteMasterModel WithClientDetails(this FuelQuoteMasterModel model, ClientMasterModel client)
        {
            model.ClientId = client.Id;
            model.ClientName = client.FullName;

            return model;
        }

        public static FuelQuoteMasterModel WithGallonsRequested(this FuelQuoteMasterModel model, int gallonsRequested)
        {
            model.GallonsRequested = gallonsRequested;

            return model;
        }

        public static FuelQuoteMasterModel WithTotalAmountDue(this FuelQuoteMasterModel model, int amount)
        {
            model.TotalAmountDue = amount;

            return model;
        }

        public static FuelQuoteMasterModel WithSuggestedPrice(this FuelQuoteMasterModel model, int price)
        {
            model.SuggestedPrice = price;

            return model;
        }
    }
}
