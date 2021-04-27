using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CookbookAPI.ViewModels.Areas;
using CookbookAPI.ViewModels.Categories;
using FluentAssertions;
using Xunit;

namespace CookbookAPI.Tests.Integration
{
    public class CategoriesControllerTests : BaseIntegrationTest
    {
        [Fact]
        public async Task GetAll_WithoutValidAuthorizeHeader_ShouldReturnUnauthorized()
        {
            //Arrange

            //Act
            var response = await _testClient.GetAsync("/api/categories");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetAll_WithValidAuthorizeHeader_ShouldReturnCategories()
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await _testClient.GetAsync("/api/categories");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<GetAllCategoriesVm>()).Categories.Count.Should().BeGreaterOrEqualTo(1);
        }
    }
}
