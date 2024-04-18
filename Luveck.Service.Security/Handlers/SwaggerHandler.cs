using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace Luveck.Service.Security.Handlers
{
    public class SwaggerHandler
    {
        public static void SwaggerConfig(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("ApiSecurityAccount", new OpenApiInfo()
                {
                    Title = "Api Security Account",
                    Version = "v1",
                    Description = "Backend Account",
                });
                c.SwaggerDoc("ApiSecurityRoles", new OpenApiInfo()
                {
                    Title = "Api Security Roles",
                    Version = "v1",
                    Description = "Backend Roles",
                });
                c.SwaggerDoc("ApiSecurityModuleRole", new OpenApiInfo()
                {
                    Title = "Api Security Module by role",
                    Version = "v1",
                    Description = "Backend Module by role Roles",
                });
                c.SwaggerDoc("ApiSecurityModule", new OpenApiInfo()
                {
                    Title = "Api Security Module",
                    Version = "v1",
                    Description = "Backend Module",
                });
                c.SwaggerDoc("ApiAudit", new OpenApiInfo()
                {
                    Title = "Api Audit site",
                    Version = "v1",
                    Description = "Backend Audit site",
                });

                //c.AddSecurityDefinition("Bearer",
                //   new OpenApiSecurityScheme()
                //   {
                //       Description = "Autenticacion JWT (Bearer)",
                //       Type = SecuritySchemeType.Http,
                //       Scheme = "Bearer"
                //   });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },new List<string>()
                }
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
            });
        }

        public static void UseSwagger(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/ApiSecurityAccount/swagger.json", "Api Security Account");
                c.SwaggerEndpoint("/swagger/ApiSecurityRoles/swagger.json", "Api Security Roles");
                c.SwaggerEndpoint("/swagger/ApiSecurityModuleRole/swagger.json", "Api Security Module by role");
                c.SwaggerEndpoint("/swagger/ApiSecurityModule/swagger.json", "Api Security Module");
                c.SwaggerEndpoint("/swagger/ApiAudit/swagger.json", "Api Audit site");
            });

            //app.UseSwagger();

            //app.UseSwaggerUI(options =>
            //{
            //    string swaggerJsonBasePath = string.IsNullOrWhiteSpace(options.RoutePrefix) ? "." : "..";
            //    options.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1.0/swagger.json", "Get Project Web Api");
            //});
        }
    }
}
