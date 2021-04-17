using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSwag;
using NSwag.Generation.Processors.Security;

namespace CookbookAPI.Extensions.StartupInstallers
{
    public static class SwaggerInstaller
    {
        public static void InstallSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerDocument(document =>
            {
                document.Title = "Cookbook API Documentation";
                document.DocumentName = "swagger";
                document.OperationProcessors.Add(new OperationSecurityScopeProcessor("jwt"));
                document.DocumentProcessors.Add(new SecurityDefinitionAppender("jwt", new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Description = "JWT Token - remember to add 'Bearer ' before the token",
                }));
            });
        }
    }
}
