using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IProductService
    {
        Task<ResponseDTO?> GetAllProductAsync();
        Task<ResponseDTO?> GetProductByIdAsync(int productId);
        Task<ResponseDTO?> GetProductByNameAsync(string productName);
        Task<ResponseDTO?> CreateProductAsync(ProductDTO productDTO);
        Task<ResponseDTO?> UpdateProductAsync(ProductDTO productDTO);
        Task<ResponseDTO?> DeleteProductAsync(int productId);
    }
}
