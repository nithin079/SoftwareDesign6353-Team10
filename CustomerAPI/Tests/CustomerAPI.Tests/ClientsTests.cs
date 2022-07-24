using CustomerAPI.Tests.Common;
using FluentAssertions;
using Repository.Model;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TestUtilities.Builders;
using TestUtilities.Common;
using TestUtilities.Data;
using Xunit;

namespace CustomerAPI.Tests
{
    [Collection("Sequential")]
    public class ClientsTests : IClassFixture<Feature>
    {
        private readonly Feature _feature;

        public ClientsTests(Feature feature)
        {
            _feature = feature;
        }

        //Gets

        [Fact]
        public void GetAllCustomers_Should_Not_Be_Empty_But_Should_Contain_Zero_Records()
        {
            _feature
                .Initialize()
                .GivenACleanSlate()
                .WhenIGetAllCustomers()
                .ThenTheResponseShouldMatch<Feature, IEnumerable<ClientMasterModel>>(HttpStatusCode.OK, result =>
                {
                    result.Should().NotBeNull().And.BeEmpty();
                    result.Count().Should().Be(0);
                });
        }

        [Fact]
        public void Get_Customer_By_UserName_Should_Return_Failure_If_Customer_Does_Not_Exist()
        {
            _feature
                .Initialize()
                .GivenACleanSlate()
                .WhenIGetCustomerWithUsername(TestData.UserName)
                .ThenTheResponseShouldMatch<Feature, string>(HttpStatusCode.OK, result =>
                {
                    result.Should().BeEmpty();
                });
        }

        [Fact]
        public void Get_Customer_By_UserName_Should_Return_Success_If_Customer_Exists()
        {
            _feature
                .Initialize()
                .GivenACleanSlate()
                .WhenIAddCustomer(new ClientMasterModel().WithDefaultValues())
                .WhenIGetCustomerWithUsername(TestData.UserName)
                .ThenTheResponseShouldMatch<Feature, ClientMasterModel>(HttpStatusCode.OK, result =>
                {
                    result.Should().NotBeNull();
                    result.UserName.Should().Be(TestData.UserName);
                });
        }

        //Adds

        [Fact]
        public void If_We_Add_One_Customer_When_We_GetAllCustomers_The_Record_Should_Contain_1_Customer()
        {
            _feature
                .Initialize()
                .GivenACleanSlate()
                .WhenIAddCustomer(
                    new ClientMasterModel().WithDefaultValues()
                )
                .WhenIGetAllCustomers()
                .ThenTheResponseShouldMatch<Feature, IEnumerable<ClientMasterModel>>(HttpStatusCode.OK, result =>
                {
                    result.Should().NotBeNull().And.NotBeEmpty();
                    result.Count().Should().Be(1);
                });
        }

        [Fact]
        public void If_We_Add_Two_Customer_When_We_GetAllCustomers_The_Record_Should_Contain_2_Customers()
        {
            _feature
                .Initialize()
                .GivenACleanSlate()
                .WhenIAddCustomer(
                    new ClientMasterModel().WithDefaultValues()
                )
                .WhenIGetAllCustomers()
                .WhenIAddCustomer(
                    new ClientMasterModel().WithNewValues()
                )
                .WhenIGetAllCustomers()
                .ThenTheResponseShouldMatch<Feature, IEnumerable<ClientMasterModel>>(HttpStatusCode.OK, result =>
                {
                    result.Should().NotBeNull().And.NotBeEmpty();
                    result.Count().Should().Be(2);
                });
        }

        //register
        [Fact]
        public void If_We_Register_The_Same_Customer_We_Should_Get_A_Conflict()
        {
            _feature
                .Initialize()
                .GivenACleanSlate()
                .WhenIRegisterACustomer(
                    new ClientMasterModel().WithDefaultValues()
                )
                .ThenTheResponseShouldMatch<Feature, ClientMasterModel>(HttpStatusCode.OK, result =>
                {
                    result.Should().NotBeNull();
                    result.UserName.Should().Be(TestData.UserName);
                    result.PasswordHash.Should().Be(TestData.PasswordHash);
                })
                .WhenIRegisterACustomer(
                    new ClientMasterModel().WithDefaultValues()
                )
                .ThenTheResponseShouldMatch<Feature, string>(HttpStatusCode.OK, result =>
                {
                    result.Should().NotBeNull().And.NotBeEmpty();
                    result.Should().Be("Client Already Exists");
                });
        }

        [Fact]
        public void If_We_Register_1_Customer_We_Should_Get_A_SuccessfulMessage()
        {
            _feature
                .Initialize()
                .GivenACleanSlate()
                .WhenIRegisterACustomer(
                    new ClientMasterModel().WithDefaultValues()
                )
                .ThenTheResponseShouldMatch<Feature, ClientMasterModel>(HttpStatusCode.OK, result =>
                {
                    result.Should().NotBeNull();
                    result.UserName.Should().Be(TestData.UserName);
                    result.PasswordHash.Should().Be(TestData.PasswordHash);
                });
        }

        //Authenticate
        [Fact]
        public void When_We_Authenticate_A_Client_With_The_Right_Details_We_Should_Get_A_Success()
        {
            _feature
                .Initialize()
                .GivenACleanSlate()
                .WhenIRegisterACustomer(new ClientMasterModel().WithDefaultValues())
                .WhenIAuthenticate(new LoginRequestModel().WithDefaultValuesFrom(new ClientMasterModel().WithDefaultValues()))
                .ThenTheResponseShouldMatch<Feature, ClientMasterModel>(HttpStatusCode.OK, result =>
                {
                    result.Should().NotBeNull();
                    result.UserName.Should().Be(TestData.UserName);
                });
        }

        [Fact]
        public void When_We_Authenticate_A_Client_With_The_Wrong_Details_We_Should_Get_A_Success_But_With_Invalid_Message()
        {
            _feature
                .Initialize()
                .GivenACleanSlate()
                .WhenIRegisterACustomer(new ClientMasterModel().WithDefaultValues())
                .WhenIAuthenticate(new LoginRequestModel().WithDefaultValuesFrom(new ClientMasterModel().WithNewValues()))
                .ThenTheResponseShouldMatch<Feature, string>(HttpStatusCode.OK, result =>
                {
                    result.Should().NotBeNull();
                    result.Should().Be("Invalid Username or Password");
                });
        }

        //Update
        [Fact]
        public void When_We_Update_A_Client_We_Should_Be_Able_To_See_Changes_When_We_Retrieve_It()
        {
            var newFullName = "Testing Fullname";
            var clientModel = new ClientMasterModel().WithDefaultValues();

            _feature
                .Initialize()
                .GivenACleanSlate()
                .WhenIAddCustomer(clientModel)
                .WhenIGetCustomerWithUsername(clientModel.UserName)
                .ThenTheResponseShouldMatch<Feature, ClientMasterModel>(HttpStatusCode.OK, result =>
                {
                    result.Should().NotBeNull();
                    clientModel.Id = result.Id;
                })
                .WhenIUpdateACustomer(clientModel.WithUpdatedFullName(newFullName))
                .ThenTheResponseShouldMatch<Feature, ClientMasterModel>(HttpStatusCode.OK, result =>
                {
                    result.Should().NotBeNull();
                    result.FullName.Should().Be(newFullName);
                })
                .WhenIGetCustomerWithUsername(TestData.UserName)
                .ThenTheResponseShouldMatch<Feature, ClientMasterModel>(HttpStatusCode.OK, result =>
                {
                    result.Should().NotBeNull();
                    result.FullName.Should().Be(newFullName);
                });
        }

        [Fact]
        public void When_We_Update_A_Client_That_Does_Not_Exist()
        {
            var newFullName = "Testing Fullname";
            var clientModel = new ClientMasterModel().WithDefaultValues();

            _feature
                .Initialize()
                .GivenACleanSlate()
                .WhenIUpdateACustomer(clientModel.WithUpdatedFullName(newFullName))
                .ThenTheResponseShouldMatch<Feature, string>(HttpStatusCode.OK, result =>
                {
                    result.Should().NotBeNull();
                    result.Should().Be(string.Empty);
                });
        }


        //delete
        //There is no sp_DeleteCustomer in the database. Skipping the tests for Deletion
    }
	
}
