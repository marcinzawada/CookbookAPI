using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CookbookAPI.ViewModels.Areas;
using FluentAssertions;
using Xunit;

namespace CookbookAPI.Tests.Integration
{
    public class AreasControllerTests : BaseIntegrationTest
    {
        [Fact]
        public async Task GetAll_WithoutValidAuthorizeHeader_ShouldReturnUnauthorized()
        {
            //Arrange

            //Act
            var response = await _testClient.GetAsync("/api/areas");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetAll_WithValidAuthorizeHeader_ShouldReturnAreas()
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await _testClient.GetAsync("/api/areas");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<GetAllAreaVm>()).Areas.Count.Should().BeGreaterOrEqualTo(1);
        }

        [Fact]
        public async Task Get_WithValidAreaId_ShouldReturnAreaWithRecipes()
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await _testClient.GetAsync("/api/areas/1/recipes");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var body = await response.Content.ReadAsAsync<GetAreaVm>();
            body.Name.Should().NotBeNullOrEmpty();
            body.Id.Should().BeGreaterOrEqualTo(1);
        }

        [Theory]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        [InlineData(0)]
        public async Task Get_InvalidAreaId_ShouldReturnNotFound(int id)
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await _testClient.GetAsync($"/api/areas/{id}/recipes");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
