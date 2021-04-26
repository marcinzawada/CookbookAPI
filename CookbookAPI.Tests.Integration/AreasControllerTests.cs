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
            (await response.Content.ReadAsAsync<GetAreaVm>()).Areas.Count.Should().BeGreaterOrEqualTo(1);
        }
    }
}
