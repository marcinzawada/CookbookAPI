using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using CookbookAPI.Data;
using CookbookAPI.Entities;
using CookbookAPI.Requests.Account;
using CookbookAPI.Tests.Integration.Helpers;
using CookbookAPI.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace CookbookAPI.Tests.Integration
{
    public class BaseIntegrationTest
    {
        protected readonly HttpClient _testClient;
        protected static Faker _faker = new Faker("en");

        public BaseIntegrationTest()
        {
            var factory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var descriptor = services.Single(
                            d => d.ServiceType ==
                                 typeof(DbContextOptions<CookbookDbContext>));

                        services.Remove(descriptor);

                        services.AddDbContext<CookbookDbContext>(options =>
                        {
                            options.UseInMemoryDatabase("InMemoryDatabaseForTesting");
                        });

                        services.AddScoped<DatabaseForTestSeeder>();
                        services.AddScoped<PasswordHasher<User>>();

                        var sp = services.BuildServiceProvider();

                        using (var scope = sp.CreateScope())
                        {
                            var scopedServices = scope.ServiceProvider;
                            var db = scopedServices.GetRequiredService<CookbookDbContext>();
                            var seeder = scopedServices.GetRequiredService<DatabaseForTestSeeder>();
                            var logger = scopedServices
                                .GetRequiredService<ILogger<BaseIntegrationTest>>();

                            db.Database.EnsureCreated();

                            try
                            {
                                seeder.Seed();
                            }
                            catch (Exception ex)
                            {
                                logger.LogError(ex, "An error occurred seeding the " +
                                                    "database with test messages. Error: {Message}", ex.Message);
                            }
                        }
                    });
                });

            _testClient = factory.CreateClient();
        }

        protected async Task AuthenticateAsync()
        {
            _testClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtAsync());
        }

        private async Task<string> GetJwtAsync()
        {
            var response = await _testClient.PostAsJsonAsync("/api/account/login", new LoginRequest
            {
                Email = "test@gmail.com",
                Password = "Pass123!"
            });

            var loginVm = await response.Content.ReadAsAsync<LoginVm>();
            return loginVm.Token;
        }
    }
}
