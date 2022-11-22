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
            var category = await _db.Category.FirstOrDefaultAsync(x => x.Id == productDto.IdCategory);
            if (category == null) return null;
            Product product = _mapper.Map<Product>(productDto);
            product.category = category;
            product.UpdateBy = productDto.UpdateBy;
            product.UpdateDate = DateTime.Now;

            if (product.Id > 0)
            {
                _db.Product.Update(product);
            }
            else
            {
                var p = await _db.Product.FirstOrDefaultAsync(x => x.Name.Equals(productDto.Name, StringComparison.OrdinalIgnoreCase));                
                product.CreateBy = productDto.UpdateBy;
                product.CreationDate = DateTime.Now;

                productDto = _mapper.Map<ProductDto>(product);
                productDto.IdCategory = product.category.Id;
                productDto.NameCategory = product.category.Name;
                if (p != null) return productDto;
                _db.Product.Add(product);
            }
            await _db.SaveChangesAsync();

            return productDto;
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
                              isDeleted = prod.state,
                              IdCategory = cat.Id,
                              NameCategory = cat.Name

                          })).FirstOrDefaultAsync();
        }    

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            return await(from prod in _db.Product
                         join cat in _db.Category on prod.category.Id equals cat.Id          
                         select (new ProductDto
                         {
                             Id = prod.Id,
                             Name = prod.Name,
                             Description = prod.Description,
                             presentation = prod.presentation,
                             Quantity = prod.Quantity,
                             TypeSell = prod.TypeSell,
                             Cost = prod.Cost,
                             isDeleted = prod.state,
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
                             where prod.category.Id == idCategory
                             select (new ProductDto
                             {
                                 Id = prod.Id,
                                 Name = prod.Name,
                                 Description = prod.Description,
                                 presentation = prod.presentation,
                                 Quantity = prod.Quantity,
                                 TypeSell = prod.TypeSell,
                                 isDeleted = prod.state,
                                 Cost = prod.Cost,
                                 IdCategory = cat.Id,
                                 NameCategory = cat.Name

                             })).ToListAsync();
            }
            return null;
        }
    }
}
