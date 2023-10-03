using Luveck.Service.Administration.DTO;
using Luveck.Service.Administration.DTO.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luveck.Service.Administration.Repository.IRepository
{
    public interface IProductRepository
    {
        Task<List<ProductResponseDto>> GetProducts();
        Task<List<ProductResponseDto>> GetProductsByCategory(int idCategory);
        Task<ProductResponseDto> GetProductById(int id);
        Task<ProductResponseDto> GetProductByName(string name);
        Task<ProductResponseDto> GetProductByBarcode(string barcode);
        Task<ProductResponseDto> CreateProduct(ProductRequestDto productDto, string user);
        Task<ProductResponseDto> UpdateProduct(ProductRequestDto productDto, string user);
        Task<bool> deleteProduct(int id, string user);
        Task<bool> deleteImage(string fileName);
        Task<bool> LoadImageAsync(FileRequestDto requestFile);
    }
}