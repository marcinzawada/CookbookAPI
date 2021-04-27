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

        [Fact]
        public async Task Get_WithValidCategoryId_ShouldReturnCategoryWithRecipes()
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await _testClient.GetAsync("/api/categories/1/recipes");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var body = await response.Content.ReadAsAsync<GetCategoryVm>();
            body.Name.Should().NotBeNullOrEmpty();
            body.Id.Should().BeGreaterOrEqualTo(1);
        }

        [Theory]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        [InlineData(0)]
        public async Task Get_InvalidCategoryId_ShouldReturnNotFound(int id)
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await _testClient.GetAsync($"/api/categories/{id}/recipes");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
