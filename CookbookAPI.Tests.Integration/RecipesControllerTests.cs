using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CookbookAPI.DTOs;
using CookbookAPI.Requests.Account;
using CookbookAPI.ViewModels;
using CookbookAPI.ViewModels.Recipes;
using FluentAssertions;
using Microsoft.AspNetCore.WebUtilities;
using Xunit;

namespace CookbookAPI.Tests.Integration
{
    public class RecipesControllerTests : BaseIntegrationTest
    {
        [Fact]
        public async Task GetAll_WithoutValidAuthorizeHeader_ShouldReturnUnauthorized()
        {
            //Arrange

            //Act
            var response = await _testClient.GetAsync("/api/recipes");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Theory]
        [InlineData("", 1, 5, "Area", "asc")]
        [InlineData("Breakfast", 1, 25, "Category", "desc")]
        public async Task GetAll_WithValidAuthorizeHeaderAndValidQuery_ShouldReturnRecipes(string searchPhrase,
            int pageNumber, int pageSize, string sortBy, string sortDirection)
        {
            //Arrange
            await AuthenticateAsync();
            var query = Dictionary(searchPhrase, pageNumber, pageSize, sortBy, sortDirection);

            //Act
            var response = await _testClient.GetAsync(QueryHelpers.AddQueryString("/api/recipes", query));

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<PaginatedList<RecipeDto>>()).Items.Count.Should().BeGreaterOrEqualTo(1);
        }

        [Theory]
        [InlineData("", 0, 5, "Area", "asc")]
        [InlineData("", 1, 6, "Area", "asc")]
        [InlineData("", 1, 10, "Test", "asc")]
        [InlineData("", 1, 10, "Category", "test")]
        public async Task GetAll_WithInvalidQuery_ShouldReturnBadRequest(string searchPhrase, int pageNumber, int pageSize,
            string sortBy, string sortDirection)
        {
            //Arrange
            await AuthenticateAsync();
            var query = Dictionary(searchPhrase, pageNumber, pageSize, sortBy, sortDirection);


            //Act
            var response = await _testClient.GetAsync(QueryHelpers.AddQueryString("/api/recipes", query));

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        private static Dictionary<string, string> Dictionary(string searchPhrase, int pageNumber, int pageSize, string sortBy,
            string sortDirection)
        {
            return new Dictionary<string, string>
            {
                ["SearchPhrase"] = searchPhrase,
                ["PageNumber"] = pageNumber.ToString(),
                ["PageSize"] = pageSize.ToString(),
                ["SortBy"] = sortBy,
                ["SortDirection"] = sortDirection
            };
        }

        [Fact]
        public async Task Get_WithWrongId_ShouldReturnNotFound()
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await _testClient.GetAsync($"/api/recipes/{int.MaxValue}");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Get_WithValidId_ShouldReturnRecipe()
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await _testClient.GetAsync("/api/recipes/1");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<GetRecipeVm>()).Recipe.Id.Should().Be(1);

        }

    }
}
