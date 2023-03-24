using Luveck.Service.Security.Data;
using Luveck.Service.Security.Handlers;
using Luveck.Service.Security.Models;
using Luveck.Service.Security.Services;
using Luveck.Service.Security.Utils.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Globalization;

namespace Luveck.Service.Security
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddAutoMapper(typeof(Startup));
            services.AddControllersWithViews();
            services.AddDistributedMemoryCache();
            services.AddSession();

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

            #region Configurar mail
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            #endregion

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

            #region IdentityCore

            var builder = services.AddIdentityCore<User>(opt =>
            {
                opt.Password.RequiredLength = 8;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequiredUniqueChars = 1;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                opt.Lockout.MaxFailedAccessAttempts = 3;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.TryAddSingleton<ISystemClock, SystemClock>();

            #endregion IdentityCore

            #region Jwt Configuration

            IConfigurationSection jwtAppSettings = Configuration.GetSection("JWTSettings");
            services.Configure<JwtSetting>(jwtAppSettings);

            JwtConfigurationHandler.ConfigureJwtAuthentication(services, jwtAppSettings);

            #endregion Jwt Configuration
            //services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Defaultconnection")));
            //services.AddScoped<IModuleRoleRepository, ModuleRoleRepository>();
            //services.AddScoped<IModuleRepository, ModuleRepository>();
            //services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(options =>
            //    {
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuerSigningKey = true,
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
            //            ValidateIssuer = false,
            //            ValidateAudience = false,
            //            ValidateLifetime = true,
            //        };
            //    });
            //services.AddTransient<IEmailSender, MailJetEmailSender>(); // Para envio automatico de correos


            ////Envio por correo personal
            //services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            //services.AddTransient<IMailService, Services.MailService>();

            //services.AddAutoMapper(typeof(MapperConfigAdministration));
            //services.AddControllers();
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("ApiSecurityAccount", new OpenApiInfo()
            //    {
            //        Title = "Api Security Account",
            //        Version = "v1",
            //        Description = "Backend Account",
            //    });
            //    c.SwaggerDoc("ApiSecurityRoles", new OpenApiInfo()
            //    {
            //        Title = "Api Security Roles",
            //        Version = "v1",
            //        Description = "Backend Roles",
            //    });
            //    c.SwaggerDoc("ApiSecurityModuleRole", new OpenApiInfo()
            //    {
            //        Title = "Api Security Module by role",
            //        Version = "v1",
            //        Description = "Backend Module by role Roles",
            //    });

            //    c.SwaggerDoc("ApiSecurityModule", new OpenApiInfo()
            //    {
            //        Title = "Api Security Module",
            //        Version = "v1",
            //        Description = "Backend Module",
            //    });

            //    c.AddSecurityDefinition("Bearer",
            //       new OpenApiSecurityScheme()
            //       {
            //           Description = "Autenticacion JWT (Bearer)",
            //           Type = SecuritySchemeType.Http,
            //           Scheme = "Bearer"
            //       });
            //    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
            //    {
            //        new OpenApiSecurityScheme
            //        {
            //            Reference = new OpenApiReference
            //            {
            //                Id = "Bearer",
            //                Type = ReferenceType.SecurityScheme
            //            }
            //        },new List<string>()
            //    }
            //    });
            //});
            //services.AddCors(options =>
            //{
            //    options.AddDefaultPolicy(
            //        policy =>
            //        {
            //            policy.WithOrigins("http://localhost:4200/", "http://localhost:44349/", "luveck.azurewebsites.net", "*");
            //        });
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

            //app.UseSwagger();
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/ApiSecurityAccount/swagger.json", "Api Security Account");
            //    c.SwaggerEndpoint("/swagger/ApiSecurityRoles/swagger.json", "Api Security Roles");
            //    c.SwaggerEndpoint("/swagger/ApiSecurityModuleRole/swagger.json", "Api Security Module by role");
            //    c.SwaggerEndpoint("/swagger/ApiSecurityModule/swagger.json", "Api Security Module");
            //});

            //app.Use(async (context, next) =>
            //{
            //    var token = context.Session.GetString("token");
            //    if (!string.IsNullOrEmpty(token))
            //    {
            //        context.Request.Headers.Add("Authorization", "Bearer " + token);
            //    }
            //    await next();
            //});
        }
    }
}
