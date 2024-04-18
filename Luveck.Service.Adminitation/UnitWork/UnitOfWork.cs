using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;
using Luveck.Service.Administration.Data;
using Luveck.Service.Administration.Models;
using Luveck.Service.Administration.Repository;

namespace Luveck.Service.Administration.UnitWork
{
    [ExcludeFromCodeCoverage]
    public class UnitOfWork : IUnitOfWork
    {

        #region Attributes
        private readonly AppDbContext _contextSQL;
        #endregion Attributes

        #region Constructor
        public UnitOfWork(AppDbContext contextSQL)
        {
            _contextSQL = contextSQL;
        }
        #endregion Constructor


        #region Repository
        private Repository<Country> countryRepository;
        private Repository<Department> departmentRepository;
        private Repository<City> cityRepository;
        private Repository<Category> categoryRepository;
        private Repository<Medical> medicalRepository;
        private Repository<Patology> patologyRepository;
        private Repository<Product> productRepository;
        private Repository<Pharmacy> pharmacyRepository;
        private Repository<ProductChangeRule> productChangeRuleRepository;
        private Repository<Purchase> purchaseRepository;
        private Repository<ProductPurchase> productPurchaseRepository;
        private Repository<ExchangedProduct> exchangedProductRepository;
        private Repository<MassiveRemainder> massiveRepository;
        private Repository<ImageProduct> imageProductRepository;
        public Repository<Country> CountryRepository
        {
            get
            {
                if (this.countryRepository == null)
                {
                    this.countryRepository = new Repository<Country>(_contextSQL);
                }

                return this.countryRepository;
            }
        }
        public Repository<Department> DepartmentRepository
        {
            get
            {
                if (this.departmentRepository == null)
                {
                    this.departmentRepository = new Repository<Department>(_contextSQL);
                }

                return this.departmentRepository;
            }
        }
        public Repository<City> CityRepository
        {
            get
            {
                if (this.cityRepository == null)
                {
                    this.cityRepository = new Repository<City>(_contextSQL);
                }

                return this.cityRepository;
            }
        }
        public Repository<Category> CategoryRepository
        {
            get
            {
                if (this.categoryRepository == null)
                {
                    this.categoryRepository = new Repository<Category>(_contextSQL);
                }

                return this.categoryRepository;
            }
        }
        public Repository<Medical> MedicalRepository
        {
            get
            {
                if (this.medicalRepository == null)
                {
                    this.medicalRepository = new Repository<Medical>(_contextSQL);
                }

                return this.medicalRepository;
            }
        }
        public Repository<Patology> PatologyRepository
        {
            get
            {
                if (this.patologyRepository == null)
                {
                    this.patologyRepository = new Repository<Patology>(_contextSQL);
                }

                return this.patologyRepository;
            }
        }
        public Repository<Product> ProductRepository
        {
            get
            {
                if (this.productRepository == null)
                {
                    this.productRepository = new Repository<Product>(_contextSQL);
                }

                return this.productRepository;
            }
        }
        public Repository<Pharmacy> PharmacyRepository
        {
            get
            {
                if (this.pharmacyRepository == null)
                {
                    this.pharmacyRepository = new Repository<Pharmacy>(_contextSQL);
                }

                return this.pharmacyRepository;
            }
        }
        public Repository<ProductChangeRule> ProductChangeRuleRepository
        {
            get
            {
                if (this.productChangeRuleRepository == null)
                {
                    this.productChangeRuleRepository = new Repository<ProductChangeRule>(_contextSQL);
                }

                return this.productChangeRuleRepository;
            }
        }
        public Repository<Purchase> PurchaseRepository
        {
            get
            {
                if (this.purchaseRepository == null)
                {
                    this.purchaseRepository = new Repository<Purchase>(_contextSQL);
                }

                return this.purchaseRepository;
            }
        }
        public Repository<ProductPurchase> ProductPurchaseRepository
        {
            get
            {
                if (this.productPurchaseRepository == null)
                {
                    this.productPurchaseRepository = new Repository<ProductPurchase>(_contextSQL);
                }

                return this.productPurchaseRepository;
            }
        }
        public Repository<ExchangedProduct> ExchangedProductRepository
        {
            get
            {
                if (this.exchangedProductRepository == null)
                {
                    this.exchangedProductRepository = new Repository<ExchangedProduct>(_contextSQL);
                }

                return this.exchangedProductRepository;
            }
        }
        public Repository<MassiveRemainder> MassiveRepository
        {
            get
            {
                if (this.massiveRepository == null)
                {
                    this.massiveRepository = new Repository<MassiveRemainder>(_contextSQL);
                }

                return this.massiveRepository;
            }
        }
        public Repository<ImageProduct> ImageProductRepository
        {
            get
            {
                if (this.imageProductRepository == null)
                {
                    this.imageProductRepository = new Repository<ImageProduct>(_contextSQL);
                }

                return this.imageProductRepository;
            }
        }
        #endregion


        public async Task<int> SaveAsync()
        {
            int result = 0;
            result = await _contextSQL.SaveChangesAsync();

            return result;
        }

    }
}
