using Luveck.Service.Administration.Data;
using Luveck.Service.Administration.Handlers;
using Luveck.Service.Administration.Mapper;
using Luveck.Service.Administration.Repository;
using Luveck.Service.Administration.Repository.IRepository;
using Luveck.Service.Administration.Utils.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Globalization;

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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddControllers();
            services.AddAutoMapper(typeof(Startup));
            services.AddControllersWithViews();
            services.AddDistributedMemoryCache();
            services.AddSession();

            #region Jwt Configuration
            IConfigurationSection jwtAppSettings = Configuration.GetSection("JWTSettings");
            services.Configure<JwtSetting>(jwtAppSettings);
            JwtConfigurationHandler.ConfigureJwtAuthentication(services, jwtAppSettings);
            #endregion Jwt Configuration
            
            #region Swagger
            SwaggerHandler.SwaggerConfig(services);
            //services.AddApiVersioning();
            #endregion

            #region Context SQL Server
            //services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Defaultconnection")));
            services.AddDbContext<AppDbContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("Defaultconnection"),
               providerOptions => providerOptions.EnableRetryOnFailure()));

            #endregion Context SQL Server

            #region Register (dependency injection)

            DependencyInyectionHandler.DependencyInyectionConfig(services);

            #endregion Register (dependency injection)

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                builder => builder.SetIsOriginAllowed(_ => true)
                .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var cultureInfo = new CultureInfo("es-CO");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            app.UseCors("CorsPolicy");
            app.UseHsts();
            app.UseHttpsRedirection();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }

            app.UseCors("CorsPolicy");
            app.UseSession();
            SwaggerHandler.UseSwagger(app);
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
