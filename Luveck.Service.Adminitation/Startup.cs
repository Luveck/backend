using Luveck.Service.Administration.Data;
using Luveck.Service.Administration.Mapper;
using Luveck.Service.Administration.Repository;
using Luveck.Service.Administration.Repository.IRepository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Luveck.Service.Administration
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Defaultconnection")));
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddAutoMapper(typeof(MapperConfigAdministration));
            services.AddControllers();
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
            });
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:4200", "http://localhost:4349/", "luveck.azurewebsites.net", "*");
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/ApiAdminCountry/swagger.json", "Api Administration Country");
                c.SwaggerEndpoint("/swagger/ApiAdminDepartment/swagger.json", "Api Administration Department");
                c.SwaggerEndpoint("/swagger/ApiAdminCity/swagger.json", "Api Administration City");
                c.SwaggerEndpoint("/swagger/ApiAdminSBU/swagger.json", "Api Administration SBU");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseCors();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
