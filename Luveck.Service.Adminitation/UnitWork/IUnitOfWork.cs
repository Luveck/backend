using Luveck.Service.Administration.Models;
using Luveck.Service.Administration.Repository;
using System.Threading.Tasks;

namespace Luveck.Service.Administration.UnitWork
{
    public interface IUnitOfWork
    {
        #region Repository
        Repository<Country> CountryRepository { get; }
        Repository<Department> DepartmentRepository { get; }
        Repository<City> CityRepository { get; }
        Repository<Category> CategoryRepository { get; }
        Repository<Medical> MedicalRepository { get; }
        Repository<Patology> PatologyRepository { get; }
        Repository<Product> ProductRepository { get; }
        Repository<Pharmacy> PharmacyRepository { get; }
        Repository<ProductChangeRule> ProductChangeRuleRepository { get; }
        Repository<Purchase> PurchaseRepository { get; }
        Repository<ProductPurchase> ProductPurchaseRepository { get; }
        Repository<ExchangedProduct> ExchangedProductRepository { get; }
        Repository<MassiveRemainder> MassiveRepository { get; }
        Repository<ImageProduct> ImageProductRepository { get; }
        Task<int> SaveAsync();
        #endregion
    }
}
