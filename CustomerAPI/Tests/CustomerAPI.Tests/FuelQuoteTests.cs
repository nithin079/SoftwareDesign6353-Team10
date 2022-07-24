using CustomerAPI.Tests.Common;
using FluentAssertions;
using Repository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TestUtilities.Builders;
using TestUtilities.Common;
using Xunit;

namespace CustomerAPI.Tests
{
    [Collection("Sequential")]
    public class FuelQuoteTests : IClassFixture<Feature>
    {
        private readonly Feature _feature;

        public FuelQuoteTests(Feature feature)
        {
            _feature = feature;
        }

        [Fact]
        public void When_We_Get_History_For_Fuel_Quote_We_Should_Get_An_Empty_Ressult()
        {
            _feature
                .Initialize()
                .GivenACleanSlate()
                .WhenIGetHistoryOfFuelQuote(new FuelHistoryRequestModel().WithValues(1, 1))
                .ThenTheResponseShouldMatch<Feature, IEnumerable<FuelQuoteMasterModel>>(HttpStatusCode.OK, result =>
                {
                    result.Should().NotBeNull().And.BeEmpty();
                    result.Count().Should().Be(0);
                });
        }

        [Fact]
        public void When_We_Add_FuelQuote_For_A_Client_That_Does_Not_Exist()
        {
            _feature
                .Initialize()
                .GivenACleanSlate()
                .WhenIAddFuelQuoteForClient(
                    new FuelQuoteMasterModel()
                        .WithDefaultValues()
                        .WithClientDetails(new ClientMasterModel().WithNewValues())
                        .WithGallonsRequested(100)
                        .WithTotalAmountDue(10)
                        .WithSuggestedPrice(5)
                )
                .ThenTheResponseShouldMatch<Feature, string>(HttpStatusCode.OK, result =>
                {
                    result.Should().NotBeNull();
                    result.Should().Be("");
                });
        }

        [Fact]
        public void When_We_Add_A_Fuel_Quote_For_A_Client_And_When_We_Receive_The_History_We_Should_Get_It_Back()
        {
            var clientModel = new ClientMasterModel().WithDefaultValues();
            var roleId = default(int);

            _feature
                .Initialize()
                .GivenACleanSlate()
                .WhenIAddCustomer(clientModel)
                .WhenIGetCustomerWithUsername(clientModel.UserName)
                .ThenTheResponseShouldMatch<Feature, ClientMasterModel>(HttpStatusCode.OK, result =>
                {
                    result.Should().NotBeNull();
                    clientModel.Id = result.Id;
                    clientModel.Role = result.Role;
                    roleId = Convert.ToInt32(clientModel.Role);
                })
                .WhenIAddFuelQuoteForClient(
                    new FuelQuoteMasterModel()
                        .WithDefaultValues()
                        .WithClientDetails(clientModel)
                )
                .WhenIGetHistoryOfFuelQuote(new FuelHistoryRequestModel().WithValues(clientModel.Id, roleId))
                .ThenTheResponseShouldMatch<Feature, IEnumerable<FuelQuoteMasterModel>>(HttpStatusCode.OK, result =>
                {
                    result.Should().NotBeNull();
                    result.Count().Should().Be(1);
                });
        }

        [Fact]
        public void When_We_Add_2_Fuel_Quotes_For_Client1_And_Another_For_Client2_We_Should_Get_2_For_Client1()
        {
            var clientModel = new ClientMasterModel().WithDefaultValues();
            var clientModel2 = new ClientMasterModel().WithNewValues();
            var roleId = default(int);

            _feature
                .Initialize()
                .GivenACleanSlate()
                .WhenIAddCustomer(clientModel)
                .WhenIAddCustomer(clientModel2)
                .WhenIGetCustomerWithUsername(clientModel.UserName)
                .ThenTheResponseShouldMatch<Feature, ClientMasterModel>(HttpStatusCode.OK, result =>
                {
                    result.Should().NotBeNull();
                    clientModel.Id = result.Id;
                    clientModel.Role = result.Role;
                    roleId = Convert.ToInt32(clientModel.Role);
                })
                .WhenIGetCustomerWithUsername(clientModel2.UserName)
                .ThenTheResponseShouldMatch<Feature, ClientMasterModel>(HttpStatusCode.OK, result =>
                {
                    result.Should().NotBeNull();
                    clientModel2.Id = result.Id;
                    clientModel2.Role = result.Role;
                })
                .WhenIAddFuelQuoteForClient(
                    new FuelQuoteMasterModel()
                        .WithDefaultValues()
                        .WithClientDetails(clientModel)
                        .WithGallonsRequested(100)
                        .WithTotalAmountDue(10)
                        .WithSuggestedPrice(5)
                )
                .WhenIAddFuelQuoteForClient(
                    new FuelQuoteMasterModel()
                        .WithDefaultValues()
                        .WithClientDetails(clientModel)
                        .WithGallonsRequested(500)
                        .WithTotalAmountDue(50)
                        .WithSuggestedPrice(9)
                )
                .WhenIAddFuelQuoteForClient( //2nd client
                    new FuelQuoteMasterModel()
                        .WithDefaultValues()
                        .WithClientDetails(clientModel2)
                        .WithGallonsRequested(30)
                        .WithTotalAmountDue(3)
                        .WithSuggestedPrice(10)
                )
                .WhenIGetHistoryOfFuelQuote(new FuelHistoryRequestModel().WithValues(clientModel.Id, roleId))
                .ThenTheResponseShouldMatch<Feature, IEnumerable<FuelQuoteMasterModel>>(HttpStatusCode.OK, result =>
                {
                    result.Should().NotBeNull();
                    result.Count().Should().Be(2);
                });
        }

        [Fact]
        public void When_We_Add_2_Fuel_Quotes_For_Client1_And_Another_For_Client2_We_Should_Get_1_For_Client2()
        {
            var clientModel = new ClientMasterModel().WithDefaultValues();
            var clientModel2 = new ClientMasterModel().WithNewValues();
            var roleId = default(int);

            _feature
                .Initialize()
                .GivenACleanSlate()
                .WhenIAddCustomer(clientModel)
                .WhenIAddCustomer(clientModel2)
                .WhenIGetCustomerWithUsername(clientModel.UserName)
                .ThenTheResponseShouldMatch<Feature, ClientMasterModel>(HttpStatusCode.OK, result =>
                {
                    result.Should().NotBeNull();
                    clientModel.Id = result.Id;
                    clientModel.Role = result.Role;
                })
                .WhenIGetCustomerWithUsername(clientModel2.UserName)
                .ThenTheResponseShouldMatch<Feature, ClientMasterModel>(HttpStatusCode.OK, result =>
                {
                    result.Should().NotBeNull();
                    clientModel2.Id = result.Id;
                    clientModel2.Role = result.Role;
                    roleId = Convert.ToInt32(clientModel2.Role);
                })
                .WhenIAddFuelQuoteForClient(
                    new FuelQuoteMasterModel()
                        .WithDefaultValues()
                        .WithClientDetails(clientModel)
                        .WithGallonsRequested(100)
                        .WithTotalAmountDue(10)
                        .WithSuggestedPrice(5)
                )
                .WhenIAddFuelQuoteForClient(
                    new FuelQuoteMasterModel()
                        .WithDefaultValues()
                        .WithClientDetails(clientModel)
                        .WithGallonsRequested(500)
                        .WithTotalAmountDue(50)
                        .WithSuggestedPrice(9)
                )
                .WhenIAddFuelQuoteForClient( //2nd client
                    new FuelQuoteMasterModel()
                        .WithDefaultValues()
                        .WithClientDetails(clientModel2)
                        .WithGallonsRequested(30)
                        .WithTotalAmountDue(3)
                        .WithSuggestedPrice(10)
                )
                .WhenIGetHistoryOfFuelQuote(new FuelHistoryRequestModel().WithValues(clientModel2.Id, roleId))
                .ThenTheResponseShouldMatch<Feature, IEnumerable<FuelQuoteMasterModel>>(HttpStatusCode.OK, result =>
                {
                    result.Should().NotBeNull();
                    result.Count().Should().Be(1);
                });
        }
    }
}
