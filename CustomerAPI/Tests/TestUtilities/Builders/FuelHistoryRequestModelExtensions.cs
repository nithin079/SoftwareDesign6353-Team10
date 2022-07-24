using Repository.Model;

namespace TestUtilities.Builders
{
    public static class FuelHistoryRequestModelExtensions
    {
        public static FuelHistoryRequestModel WithValues(this FuelHistoryRequestModel model, int clientId, int roleId)
        {
            model.ClientId = clientId;
            model.RoleId = roleId;

            return model;
        }
    }
}
