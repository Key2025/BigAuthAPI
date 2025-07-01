using BigAuthApi.Model.Request;
using BigAuthApi.Model.Response;

namespace BigAuthApi.Service.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductResponse>> GetAllProductsAsync();

        Task<int> AddProductAsync(ProductRequest req);

        Task<ProductResponse?> GetProductByIdAsync(int id);

        Task<int> UpdateProductAsync(ProductRequest req);

        Task<int> DeleteProductAsync(int id);
    }
}