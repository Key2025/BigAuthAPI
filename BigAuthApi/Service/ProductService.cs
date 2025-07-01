using BigAuthApi.Model.Request;
using BigAuthApi.Model.Response;
using BigAuthApi.Repository.Interfaces;
using BigAuthApi.Service.Interfaces;

namespace BigAuthApi.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<int> AddProductAsync(ProductRequest req)
        {
            req.CreatedBy = Environment.UserName;
            var result = await _productRepository.AddProductsAsync(req);
            return result;
        }

        public async Task<List<ProductResponse>> GetAllProductsAsync()
        {
            var result = await _productRepository.GetAllProductsAsync();
            return result;
        }

        public async Task<ProductResponse?> GetProductByIdAsync(int id)
        {
            var result = await _productRepository.GetProductByIdAsync(id);
            return result;
        }

        public async Task<int> UpdateProductAsync(ProductRequest req)
        {
            var result = await _productRepository.UpdateProductAsync(req);
            return result;
        }

        public async Task<int> DeleteProductAsync(int id)
        {
            var result = await _productRepository.DeleteProductAsync(id);
            return result;
        }
    }
}