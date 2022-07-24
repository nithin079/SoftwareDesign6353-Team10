using Dapper;
using Microsoft.Extensions.Configuration;
using Repository.Abstract;
using Repository.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Repository.Concrete
{
    public class ClientMaster : IClientMaster
    {
        public IConfiguration _configuration { get; }

        public ClientMaster(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool Save(ClientMasterModel customerMaster)
        {
            string connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@UserName", customerMaster.UserName);
                    parameters.Add("@FullName", customerMaster.FullName);
                    parameters.Add("@Address1", customerMaster.Address1);
                    parameters.Add("@City", customerMaster.City);
                    parameters.Add("@State", customerMaster.State);
                    parameters.Add("@Zipcode", customerMaster.Zipcode);
                    parameters.Add("@PasswordHash", customerMaster.Zipcode);
                    parameters.Add("@Role", customerMaster.Role);
                    var UserData = SqlMapper.Query<ClientMasterModel>(con, "sp_AddCustomer", param: parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return true;
                }
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<ClientMasterModel> GetAllClients()
        {
            string connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                var result = SqlMapper.Query<ClientMasterModel>(con, "sp_GetAllCustomers", param: null, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
                return result;
            }
        }

        public ClientMasterModel Update(ClientMasterModel customerMaster)
        {
            string connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@ID", customerMaster.Id);
                    parameters.Add("@UserName", customerMaster.UserName);
                    parameters.Add("@FullName", customerMaster.FullName);
                    parameters.Add("@Address1", customerMaster.Address1);
                    parameters.Add("@Address2", customerMaster.Address2);
                    parameters.Add("@City", customerMaster.City);
                    parameters.Add("@State", customerMaster.State);
                    parameters.Add("@Zipcode", customerMaster.Zipcode);
                    var UserData = SqlMapper.Query<ClientMasterModel>(con, "Client_ups", param: parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return UserData;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ClientMasterModel GetUserByUsername(string userName)
        {
            string connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserName", userName);

                var result = SqlMapper.Query<ClientMasterModel>(con, "spCheckUserName", param: parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                con.Close();
                return result;
            }
        }

        public ClientMasterModel LoginCheck(LoginRequestModel loginRequestModel)
        {
            string connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserName", loginRequestModel.UserName);
                parameters.Add("@PasswordHash", loginRequestModel.PasswordHash);

                var result = SqlMapper.Query<ClientMasterModel>(con, "spLoginCheck", param: parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                con.Close();
                return result;
            }
        }

        public bool DeleteCustomer(int userId)
        {
            string connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@UserId", userId);
                    var UserData = SqlMapper.Query<int>(con, "sp_DeleteCustomer", param: parameters, commandType: CommandType.StoredProcedure);
                    return true;
                }
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public ClientMasterModel Register(ClientMasterModel customerMaster)
        {
            string connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@UserName", customerMaster.UserName);
                    parameters.Add("@PasswordHash", customerMaster.PasswordHash);
                    var UserData = SqlMapper.Query<ClientMasterModel>(con, "Client_Register", param: parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return UserData;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}