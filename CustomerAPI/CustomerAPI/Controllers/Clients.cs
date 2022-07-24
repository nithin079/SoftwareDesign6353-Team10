using CustomerAPI.Constants;
using Microsoft.AspNetCore.Mvc;
using Repository.Abstract;
using Repository.Model;
using System.Net;

namespace CustomerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Clients : ControllerBase
    {
        private readonly IClientMaster _clientMaster;

        public Clients(IClientMaster clientMaster)
        {
            _clientMaster = clientMaster;
        }

        //Get all customers from datbase
        [HttpGet("clients")]
        public Response GetAllCustomers()
        {
            var result = _clientMaster.GetAllClients();
            Response response = new Response(HttpStatusCode.OK, result, AppConstant.Success);
            return response;
        }

        [HttpGet("getclientbyid/{userName}")]
        public Response GetCustomerById(string userName)
        {
            var result = _clientMaster.GetUserByUsername(userName);
            Response response = new Response(HttpStatusCode.OK, result, AppConstant.Success);
            return response;
        }

        [HttpPost("add")]
        public Response Add([FromBody] ClientMasterModel customerMaster)
        {
            var result = _clientMaster.Save(customerMaster);
            Response response = new Response(HttpStatusCode.OK, result, AppConstant.Success);
            return response;
        }

        [HttpPost("register")]
        public Response Register([FromBody] ClientMasterModel customerMaster)
        {
            var checkUserExists = _clientMaster.GetUserByUsername(customerMaster.UserName);
            if (checkUserExists == null)
            {
                var result = _clientMaster.Register(customerMaster);
                Response response = new Response(HttpStatusCode.OK, result, AppConstant.Success);
                return response;
            }
            else
            {
                Response response = new Response(HttpStatusCode.Conflict, "Client Already Exists", AppConstant.Success);
                return response;
            }
        }

        [HttpPost("authenticate")]
        public Response authenticate([FromBody] LoginRequestModel loginRequestModel)
        {
            var checkUserExists = _clientMaster.LoginCheck(loginRequestModel);
            if (checkUserExists != null)
            {
                checkUserExists.PasswordHash = string.Empty;
                Response response = new Response(HttpStatusCode.OK, checkUserExists, AppConstant.Success);
                return response;
            }
            else
            {
                Response response = new Response(HttpStatusCode.NotFound, "Invalid Username or Password", AppConstant.Success);
                return response;
            }
        }

        [HttpPost("update")]
        public Response Update([FromBody] ClientMasterModel customerMaster)
        {
            var result = _clientMaster.Update(customerMaster);
            Response response = new Response(HttpStatusCode.OK, result, AppConstant.Success);
            return response;
        }

        [HttpDelete("delete/{userId}")]
        public Response Delete(int clientId)
        {
            var result = _clientMaster.DeleteCustomer(clientId);
            Response response = new Response(HttpStatusCode.OK, result, AppConstant.Success);
            return response;
        }
    }
}