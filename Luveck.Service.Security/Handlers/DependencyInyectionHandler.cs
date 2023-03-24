using Luveck.Service.Security.Repository.IRepository;
using Luveck.Service.Security.Repository;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity.UI.Services;
using Luveck.Service.Security.Services;
using Luveck.Service.Security.Utils.Jwt.Interface;
using Luveck.Service.Security.Utils.Jwt;
using Luveck.Service.Security.UnitWork;
using Microsoft.AspNetCore.Identity;
using Luveck.Service.Security.Utils.ErrorIdentityMessage;

namespace Luveck.Service.Security.Handlers
{
    public class DependencyInyectionHandler
    {
        [ExcludeFromCodeCoverage]
        public static void DependencyInyectionConfig(IServiceCollection services)
        {

            //Repository
            services.AddScoped<IModuleRoleRepository, ModuleRoleRepository>();
            services.AddScoped<IModuleRepository, ModuleRepository>();
            services.AddScoped<UnitOfWork, UnitOfWork>();
            services.AddScoped<UtilsJwt, UtilsJwt>();
            services.AddScoped<IdentityErrorDescriber, CustomIdentityErrorDescriber>();

            //Domain
            services.AddTransient<IAutenticationService, AutenticationService>();
            services.AddTransient<IJwtServices, JwtServices>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IMailService, MailService>();
            services.AddTransient<IModuleRepository, ModuleRepository>();
            services.AddTransient<IModuleRoleRepository, ModuleRoleRepository>();
            services.AddTransient<IAuditService, AuditService>();

            //Infraestructure
            services.AddTransient<IEmailSender, MailJetEmailSender>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IUtils, UtilsJwt>();
            services.AddTransient<IHeaderClaims, HeaderClaims>();
        }
    }
}
