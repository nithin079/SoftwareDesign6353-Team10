using Repository.Model;
using System.Collections.Generic;

namespace Repository.Abstract
{
    public interface IClientMaster
    {
        IEnumerable<ClientMasterModel> GetAllClients();

        bool Save(ClientMasterModel customerMaster);

        ClientMasterModel Update(ClientMasterModel customerMaster);

        ClientMasterModel Register(ClientMasterModel customerMaster);

        ClientMasterModel GetUserByUsername(string userName);

        bool DeleteCustomer(int userId);

        ClientMasterModel LoginCheck(LoginRequestModel loginRequestModel);
    }
}