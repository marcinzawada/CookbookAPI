using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CookbookAPI.Requests.Account;
using CookbookAPI.ViewModels.Account;
using FluentAssertions;
using Microsoft.AspNetCore.Diagnostics;
using Xunit;

namespace CookbookAPI.Tests.Integration
{
    public class AccountControllerTests : BaseIntegrationTest
    {
        [Fact]
        public async Task Register_WithCorrectData_ShouldReturnOkStatus()
        {
            //Arrange

            //Act
            var response = await _testClient.PostAsJsonAsync("/api/account/register", new RegisterRequest
            {
                Email = _faker.Internet.Email(),
                Name = "username",
                Password = "Pass123!",
                ConfirmPassword = "Pass123!"
            });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        public static IEnumerable<object[]> IncorrectRegisterData =>
            new List<object[]>
            {
                new object[] { "test@gmail.com", _faker.Person.UserName, "Pass123!", "Pass123!" },
                new object[] { "", _faker.Person.UserName, "Pass123!", "Pass123!" },
                new object[] { "test", _faker.Person.UserName, "Pass123!", "Pass123!" },
                new object[] { _faker.Internet.Email(), "" , "Pass123!", "Pass123!" },
                new object[] { _faker.Internet.Email(), _faker.Person.UserName, "", "Pass123!" },
                new object[] { _faker.Internet.Email(), _faker.Person.UserName, "Pass123", "Pass123" },
                new object[] { _faker.Internet.Email(), _faker.Person.UserName, "Pa1!", "Pa1!" },
                new object[] { _faker.Internet.Email(), _faker.Person.UserName, "pass123!", "pass123!" },
                new object[] { _faker.Internet.Email(), _faker.Person.UserName, "password!", "password!" },
                new object[] { _faker.Internet.Email(), _faker.Person.UserName, "password123", "password123" },
                new object[] { _faker.Internet.Email(), _faker.Person.UserName, "PASSWORD123!", "PASSWORD123!" },
                new object[] { _faker.Internet.Email(), _faker.Person.UserName, "Pass 123!", "Pass 123!" },
                new object[] { _faker.Internet.Email(), _faker.Person.UserName, "Pass#123!", "Pass#123!" },
                new object[] { _faker.Internet.Email(), "a", "Pass123!", "Pass123!" },
                new object[] { _faker.Internet.Email(), "very very long username", "Pass123!", "Pass123!" },
            };

        [Theory]
        [MemberData(nameof(IncorrectRegisterData))]
        public async Task Register_WithIncorrectData_ShouldReturnBadRequest(string email, string name, string password, string confirmPassword)
        {
            //Arrange

            //Act
            var response = await _testClient.PostAsJsonAsync("/api/account/register", new RegisterRequest
            {
                Email = email,
                Name = name,
                Password = password,
                ConfirmPassword = confirmPassword
            });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Login_WithCorrectCredentials_ShouldReturnOkStatus()
        {
            //Arrange

            //Act
            var response = await _testClient.PostAsJsonAsync("/api/account/login", new LoginRequest
            {
                Email = "test@gmail.com",
                Password = "Pass123!",
            });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var loginVm = await response.Content.ReadAsAsync<LoginVm>();
            loginVm.Id.Should().BeGreaterOrEqualTo(1);
            loginVm.Token.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData("","")]
        [InlineData("test@gmail.com","")]
        [InlineData("test@gmail.com", "invalidPassword")]
        [InlineData("thisEmailDoesntExist@gmail.com", "password")]
        public async Task Login_WithIncorrectCredentials_ShouldReturnBadRequest(string email, string password)
        {
            //Arrange

            //Act
            var response = await _testClient.PostAsJsonAsync("/api/account/login", new LoginRequest
            {
                Email = email,
                Password = password,
            });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
