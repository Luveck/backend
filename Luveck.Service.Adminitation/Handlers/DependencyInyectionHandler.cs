using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using Luveck.Service.Administration.UnitWork;
using Luveck.Service.Administration.Utils.Jwt;
using Luveck.Service.Administration.Repository;
using Luveck.Service.Administration.Repository.IRepository;
using Luveck.Service.Administration.Utils.Jwt.Interface;
using Luveck.Service.Administration.Services;

namespace Luveck.Service.Administration.Handlers
{
    public class DependencyInyectionHandler
    {
        [ExcludeFromCodeCoverage]
        public static void DependencyInyectionConfig(IServiceCollection services)
        {

            //Repository
            services.AddScoped<UnitOfWork, UnitOfWork>();
            services.AddScoped<UtilsJwt, UtilsJwt>();

            //Domain 
            services.AddTransient<ICountryRepository, CountryRepository>();
            services.AddTransient<IDepartmentRepository, DepartmentRepository>();
            services.AddTransient<ICityRepository, CityRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IMedicalRepository, MedicalRespository>();
            services.AddTransient<IPatologyRepository, PatologyRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IProductChangeRuleRepository, ProductChangeRuleRepository>();
            services.AddTransient<IPharmacyRepository, PharmacyRepository>();
            services.AddTransient<IProductChangeRuleRepository, ProductChangeRuleRepository>();
            services.AddTransient<IProductPurchaseRepository, ProductPurchaseRepository>();
            services.AddTransient<IExchangeProduct, ExchangeProduct>();
            services.AddTransient<IPurchaseRepository, PurchaseRepository>();
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IMailService, MailService>();

            //Infraestructure
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IUtils, UtilsJwt>();
            services.AddTransient<IHeaderClaims, HeaderClaims>();
            services.AddTransient<ISendPedingExchange, SendPedingExchange>();
            services.AddTransient<IRestService, RestService>();
        }
    }
}
