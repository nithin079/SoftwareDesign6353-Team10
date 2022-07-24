using Repository.Model;

namespace TestUtilities.Builders
{
    public static class LoginRequestModelExtensions
    {
        public static LoginRequestModel WithDefaultValuesFrom(this LoginRequestModel model, ClientMasterModel client)
        {
            model.UserName = client.UserName;
            model.PasswordHash = client.PasswordHash;

            return model;
        }
    }
}
