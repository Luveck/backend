using Luveck.Service.Administration.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luveck.Service.Administration.Repository.IRepository
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDto>> GetProducts();
        Task<IEnumerable<ProductDto>> GetProductsByCategory(int idCategory);
        Task<ProductDto> GetProduct(int id);
        Task<ProductDto> CreateUpdateProduct(ProductDto productDto);
        Task<bool> deleteProduct(int id);
    }
}