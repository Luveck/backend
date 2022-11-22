using Luveck.Service.Administration.Data;
using Luveck.Service.Administration.Mapper;
using Luveck.Service.Administration.Repository;
using Luveck.Service.Administration.Repository.IRepository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

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
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IPatologyRepository, PatologyRepository>();
            services.AddScoped<IPharmacyRepository, PharmacyRepository>();
            services.AddScoped<IMedicalRepository, MedicalRespository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IPurchaseRepository, PurchaseRepository>();
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
                c.SwaggerEndpoint("/swagger/ApiAdminCategory/swagger.json", "Api Administration Categories");
                c.SwaggerEndpoint("/swagger/ApiAdminProduct/swagger.json", "Api Administration Product");
                c.SwaggerEndpoint("/swagger/ApiAdminPatology/swagger.json", "Api Administration Patology");
                c.SwaggerEndpoint("/swagger/ApiAdminPharmacy/swagger.json", "Api Administration Pharmacy");
                c.SwaggerEndpoint("/swagger/ApiAdminMedical/swagger.json", "Api Administration Medical");
                c.SwaggerEndpoint("/swagger/ApiAdminPurchase/swagger.json", "Api Administration Purchase");
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
