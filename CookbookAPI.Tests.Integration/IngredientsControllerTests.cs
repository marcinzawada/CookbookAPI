using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CookbookAPI.DTOs;
using CookbookAPI.Requests.Ingredients;
using CookbookAPI.ViewModels;
using CookbookAPI.ViewModels.Ingredients;
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

        [Fact]
        public async Task Create_WithInvalidRequestData_ShouldReturnBadRequest()
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await _testClient.PostAsJsonAsync("/api/ingredients", new IngredientRequest
            {
                Name = ""
            });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Create_WithValidRequestData_ShouldReturnCreated()
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await _testClient.PostAsJsonAsync("/api/ingredients", new IngredientRequest
            {
                Name = "testName"
            });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task GetById_WithValidIngredientId_ShouldReturnIngredientWithRecipes()
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await _testClient.GetAsync("/api/ingredients/1/recipes");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<GetIngredientVm>()).Name.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public async Task GetById_WithInvalidIngredientId_ShouldReturnNotFound(int id)
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await _testClient.GetAsync($"/api/ingredients/{id}/recipes");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Update_WithValidRequestData_ShouldReturnOk()
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await _testClient.PutAsJsonAsync("/api/ingredients/3", new IngredientRequest
            {
                Name = "Changed name",
                Description = "Changed description"
            });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public async Task Update_WithInvalidIngredientId_ShouldReturnNotFound(int id)
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await _testClient.PutAsJsonAsync($"/api/ingredients/{id}", new IngredientRequest
            {
                Name = "Changed name",
                Description = "Changed description"
            });
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Update_IngredientThatIsUseByAnotherUser_ShouldReturnBadRequest()
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await _testClient.PutAsJsonAsync("/api/ingredients/1", new IngredientRequest
            {
                Name = "Changed name",
                Description = "Changed description"
            });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Update_WithInvalidData_ShouldReturnBadRequest()
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await _testClient.PutAsJsonAsync("/api/ingredients/1", new IngredientRequest
            {
                Name = "",
                Description = "Changed description"
            });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Update_IngredientThatBelongToOtherUser_ShouldReturnForbidden()
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await _testClient.PutAsJsonAsync("/api/ingredients/2", new IngredientRequest
            {
                Name = "Changed name",
                Description = "Changed description"
            });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Delete_WithValidRequest_ShouldReturnNoContent()
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await _testClient.DeleteAsync("/api/ingredients/3");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Theory]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public async Task Delete_WithInvalidIngredientId_ShouldReturnNotFound(int id)
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await _testClient.DeleteAsync($"/api/ingredients/{id}");
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_IngredientThatIsUseByAnotherUser_ShouldReturnBadRequest()
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await _testClient.DeleteAsync("/api/ingredients/1");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Delete_IngredientThatBelongToOtherUser_ShouldReturnForbidden()
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await _testClient.DeleteAsync("/api/ingredients/2");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}
