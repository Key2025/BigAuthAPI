using BigAuthApi.Model.Request;
using BigAuthApi.Model.Response;

namespace BigAuthApi.Repository.Interfaces
{
    public interface IProductRepository
    {
        Task<List<ProductResponse>> GetAllProductsAsync();

        Task<int> AddProductsAsync(ProductRequest req);
    }
}