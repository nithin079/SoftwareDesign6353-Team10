using Repository.Model;
using TestUtilities.Data;

namespace TestUtilities.Builders
{
    public static class ClientMasterModelExtensions
    {
        public static ClientMasterModel WithDefaultValues(this ClientMasterModel model)
        {
            model.FullName = TestData.Fullname;
            model.UserName = TestData.UserName;
            model.Address1 = TestData.Address1;
            model.Address2 = TestData.Address2;
            model.City = TestData.City;
            model.State = TestData.State;
            model.Zipcode = TestData.ZipCode;
            model.PasswordHash = TestData.PasswordHash;
            model.Role = Role.Admin;

            return model;
        }

        public static ClientMasterModel WithNewValues(this ClientMasterModel model)
        {
            model.FullName = TestData.Fullname2;
            model.UserName = TestData.UserName2;
            model.Address1 = TestData.Address1_2;
            model.Address2 = TestData.Address2_2;
            model.City = TestData.City2;
            model.State = TestData.State2;
            model.Zipcode = TestData.ZipCode2;
            model.PasswordHash = TestData.PasswordHash2;
            model.Role = Role.Admin;

            return model;
        }

        public static ClientMasterModel WithUpdatedFullName(this ClientMasterModel model, string newFullName)
        {
            model.FullName = newFullName;

            return model;
        }

        public static ClientMasterModel WithId(this ClientMasterModel model, int id)
        {
            model.Id = id;

            return model;
        }
    }
}
