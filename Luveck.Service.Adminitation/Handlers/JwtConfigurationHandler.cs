using Luveck.Service.Administration.Utils.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Luveck.Service.Administration.Handlers
{
    [ExcludeFromCodeCoverage]
    public class JwtConfigurationHandler
    {
        public static void ConfigureJwtAuthentication(IServiceCollection services, IConfigurationSection jwtAppSettings)
        {
            JwtSetting appSettings = jwtAppSettings.Get<JwtSetting>();
            var key = Encoding.UTF8.GetBytes(appSettings.securityKey);
            var secretKey = new SymmetricSecurityKey(key);

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
               .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, jwtBearerOptions =>
               {
                   jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidateAudience = false,
                       IssuerSigningKey = secretKey,
                       ValidIssuer = appSettings.validIssuer,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                   };
               });
        }
    }
}
