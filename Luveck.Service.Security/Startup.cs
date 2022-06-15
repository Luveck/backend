using Luveck.Service.Security.Data;
using Luveck.Service.Security.Mapper;
using Luveck.Service.Security.Repository;
using Luveck.Service.Security.Repository.IRepository;
using Luveck.Service.Security.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luveck.Service.Security
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
            services.AddScoped<IModuleRoleRepository, ModuleRoleRepository>();
            services.AddScoped<IModuleRepository, ModuleRepository>();
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            services.AddTransient<IEmailSender, MailJetEmailSender>(); // Para envio automatico de correos

            services.Configure<IdentityOptions>(opt =>
            {
                opt.Password.RequiredLength = 8;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequiredUniqueChars = 1;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                opt.Lockout.MaxFailedAccessAttempts = 3;
            });
            //Envio por correo personal
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddTransient<IMailService, Services.MailService>();
            services.AddAutoMapper(typeof(MapperConfigAdministration));
            services.AddControllers();
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

                c.AddSecurityDefinition("Bearer",
                   new OpenApiSecurityScheme()
                   {
                       Description = "Autenticacion JWT (Bearer)",
                       Type = SecuritySchemeType.Http,
                       Scheme = "Bearer"
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
            });
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:4200/", "http://localhost:44349/", "luveck.azurewebsites.net", "*");
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
                c.SwaggerEndpoint("/swagger/ApiSecurityAccount/swagger.json", "Api Security Account");
                c.SwaggerEndpoint("/swagger/ApiSecurityRoles/swagger.json", "Api Security Roles");
                c.SwaggerEndpoint("/swagger/ApiSecurityModuleRole/swagger.json", "Api Security Module by role");
                c.SwaggerEndpoint("/swagger/ApiSecurityModule/swagger.json", "Api Security Module");
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
