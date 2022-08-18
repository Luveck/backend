using AutoMapper;
using Luveck.Service.Administration.Data;
using Luveck.Service.Administration.Models.Dto;
using Luveck.Service.Administration.Repository.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Luveck.Service.Administration.Models;
using System;

namespace Luveck.Service.Administration.Repository
{
    public class ProductRepository : IProductRepository
    {
        public AppDbContext _db;
        public IMapper _mapper;

        public ProductRepository(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<ProductDto> CreateUpdateProduct(ProductDto productDto)
        {
            Product product = _mapper.Map<Product>(productDto);

            if (product.Id > 0)
            {
                _db.Product.Update(product);
            }
            else
            {
                _db.Product.Add(product);
            }
            await _db.SaveChangesAsync();

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<bool> deleteProduct(int id)
        {
            try
            {
                Product product = await _db.Product.FirstOrDefaultAsync(c => c.Id == id);

                if (product == null) return false;

                product.state = false;
                _db.Product.Update(product);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<ProductDto> GetProduct(int id)
        {
            return await (from prod in _db.Product
                          join cat in _db.Category on prod.category.Id equals cat.Id
                          where prod.Id == id
                          select (new ProductDto
                          {
                              Id = prod.Id,
                              Name = prod.Name,
                              Description = prod.Description,
                              presentation = prod.presentation,
                              Quantity = prod.Quantity,
                              TypeSell = prod.TypeSell,
                              Cost = prod.Cost,
                              IdCategory = cat.Id,
                              NameCategory = cat.Name

                          })).FirstOrDefaultAsync();
        }    

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            return await(from prod in _db.Product
                         join cat in _db.Category on prod.category.Id equals cat.Id
                         where prod.state == true
                         select (new ProductDto
                         {
                             Id = prod.Id,
                             Name = prod.Name,
                             Description = prod.Description,
                             presentation = prod.presentation,
                             Quantity = prod.Quantity,
                             TypeSell = prod.TypeSell,
                             Cost = prod.Cost,
                             IdCategory = cat.Id,
                             NameCategory = cat.Name

                         })).ToListAsync();
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByCategory(int idCategory)
        {
            var category = _db.Category.FirstOrDefaultAsync(x => x.Id == idCategory);
            if (category != null)
            {
                return await(from prod in _db.Product
                             join cat in _db.Category on prod.category.Id equals cat.Id
                             where prod.state == true && prod.category.Id == idCategory
                             select (new ProductDto
                             {
                                 Id = prod.Id,
                                 Name = prod.Name,
                                 Description = prod.Description,
                                 presentation = prod.presentation,
                                 Quantity = prod.Quantity,
                                 TypeSell = prod.TypeSell,
                                 Cost = prod.Cost,
                                 IdCategory = cat.Id,
                                 NameCategory = cat.Name

                             })).ToListAsync();
            }
            return null;
        }
    }
}
