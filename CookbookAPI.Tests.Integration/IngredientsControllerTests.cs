using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CookbookAPI.DTOs;
using CookbookAPI.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.WebUtilities;
using Xunit;

namespace CookbookAPI.Tests.Integration
{
    public class IngredientsControllerTests : BaseIntegrationTest
    {
        [Fact]
        public async Task GetAll_WithoutValidAuthorizeHeader_ShouldReturnUnauthorized()
        {
            //Arrange

            //Act
            var response = await _testClient.GetAsync("/api/ingredients");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Theory]
        [InlineData("", 1, 5, "Id", "asc")]
        [InlineData("tomato", 1, 25, "Name", "desc")]
        public async Task GetAll_WithValidQuery_ShouldReturnIngredients(string searchPhrase,
            int pageNumber, int pageSize, string sortBy, string sortDirection)
        {
            //Arrange
            await AuthenticateAsync();
            var query = Dictionary(searchPhrase, pageNumber, pageSize, sortBy, sortDirection);

            //Act
            var response = await _testClient.GetAsync(QueryHelpers.AddQueryString("/api/ingredients", query));

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<PaginatedList<RecipeDto>>()).Items.Count.Should().BeGreaterOrEqualTo(1);
        }

        [Theory]
        [InlineData("", 0, 5, "Name", "asc")]
        [InlineData("", 1, 6, "Name", "asc")]
        [InlineData("", 1, 10, "Test", "asc")]
        [InlineData("", 1, 10, "Name", "test")]
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

        private static Dictionary<string, string> Dictionary(string searchPhrase,
            int pageNumber, int pageSize, string sortBy, string sortDirection)
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
    }
}
