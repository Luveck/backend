using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace Luveck.Service.Administration.Handlers
{
    public class SwaggerHandler
    {
        public static void SwaggerConfig(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("ApiAdminCountry", new OpenApiInfo()
                {
                    Title = "Api Administration Country",
                    Version = "v1",
                    Description = "Backend country administration.",
                });

                c.SwaggerDoc("ApiAdminDepartment", new OpenApiInfo()
                {
                    Title = "Api Administration Department",
                    Version = "v1",
                    Description = "Backend department administration.",
                });

                c.SwaggerDoc("ApiAdminCity", new OpenApiInfo()
                {
                    Title = "Api Administration City",
                    Version = "v1",
                    Description = "Backend City administration.",
                });
                c.SwaggerDoc("ApiAdminSBU", new OpenApiInfo()
                {
                    Title = "Api SBU",
                    Version = "v1",
                    Description = "Backend SBU administration.",
                });
                c.SwaggerDoc("ApiAdminCategory", new OpenApiInfo()
                {
                    Title = "Api Categories",
                    Version = "v1",
                    Description = "Backend Categories administration.",
                });
                c.SwaggerDoc("ApiAdminProduct", new OpenApiInfo()
                {
                    Title = "Api Prooduct",
                    Version = "v1",
                    Description = "Backend Product administration.",
                });
                c.SwaggerDoc("ApiAdminPatology", new OpenApiInfo()
                {
                    Title = "Api Patology",
                    Version = "v1",
                    Description = "Backend Patology administration.",
                });
                c.SwaggerDoc("ApiAdminPharmacy", new OpenApiInfo()
                {
                    Title = "Api Pharmacy",
                    Version = "v1",
                    Description = "Backend Pharmacy administration.",
                });
                c.SwaggerDoc("ApiAdminMedical", new OpenApiInfo()
                {
                    Title = "Api Medical",
                    Version = "v1",
                    Description = "Backend Medical administration.",
                });
                c.SwaggerDoc("ApiAdminPurchase", new OpenApiInfo()
                {
                    Title = "Api Purchase",
                    Version = "v1",
                    Description = "Backend Purchase administration.",
                });
                c.SwaggerDoc("ApiRuleChage", new OpenApiInfo()
                {
                    Title = "Api Rules Products",
                    Version = "v1",
                    Description = "Backend managment rule product administration.",
                });
                c.SwaggerDoc("ApiAdminProductPurchase", new OpenApiInfo()
                {
                    Title = "Api Products on Purchase",
                    Version = "v1",
                    Description = "Backend managment product on purchase.",
                });
                c.SwaggerDoc("ApiAdminExchangeProduct", new OpenApiInfo()
                {
                    Title = "Api Exchange products",
                    Version = "v1",
                    Description = "Backend managment exchange products.",
                });
                c.SwaggerDoc("ApiReport", new OpenApiInfo()
                {
                    Title = "Api Exchange reports",
                    Version = "v1",
                    Description = "Backend managment reports.",
                });
                c.SwaggerDoc("ApiMassive", new OpenApiInfo()
                {
                    Title = "Api Masive Mail",
                    Version = "v1",
                    Description = "Backend to send massive emails.",
                });
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
                c.SwaggerEndpoint("/swagger/ApiAdminCountry/swagger.json", "Api Administration Country");
                c.SwaggerEndpoint("/swagger/ApiAdminDepartment/swagger.json", "Api Administration Department");
                c.SwaggerEndpoint("/swagger/ApiAdminCity/swagger.json", "Api Administration City");
                c.SwaggerEndpoint("/swagger/ApiAdminSBU/swagger.json", "Api Administration SBU");
                c.SwaggerEndpoint("/swagger/ApiAdminCategory/swagger.json", "Api Administration Categories");
                c.SwaggerEndpoint("/swagger/ApiAdminProduct/swagger.json", "Api Administration Product");
                c.SwaggerEndpoint("/swagger/ApiAdminPatology/swagger.json", "Api Administration Patology");
                c.SwaggerEndpoint("/swagger/ApiAdminPharmacy/swagger.json", "Api Administration Pharmacy");
                c.SwaggerEndpoint("/swagger/ApiAdminMedical/swagger.json", "Api Administration Medical");
                c.SwaggerEndpoint("/swagger/ApiAdminPurchase/swagger.json", "Api Administration Purchase");
                c.SwaggerEndpoint("/swagger/ApiRuleChage/swagger.json", "Api Administration Rules Change products");
                c.SwaggerEndpoint("/swagger/ApiAdminProductPurchase/swagger.json", "Api managment product on purchase");
                c.SwaggerEndpoint("/swagger/ApiAdminExchangeProduct/swagger.json", "Api managment exchange products");
                c.SwaggerEndpoint("/swagger/ApiReport/swagger.json", "Api managment reports");
                c.SwaggerEndpoint("/swagger/ApiMassive/swagger.json", "Api Massive mails");
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
