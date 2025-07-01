using BigAuthApi.Model.Request;
using BigAuthApi.Model.Response;
using BigAuthApi.Repository.Base;
using BigAuthApi.Repository.Interfaces;

namespace BigAuthApi.Repository.Main
{
    public class ProductRepository : BaseRepository, IProductRepository
    {
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(IDapperRepositoryBase dapperBase, ILogger<ProductRepository> logger) : base(dapperBase, logger)
        {
            _logger = logger;
        }

        public async Task<int> AddProductsAsync(ProductRequest req)
        {
            var result = await ExecuteDataAsync("SP_AddProduct", new
            {
                ProductName = req.ProductName,
                Description = req.Description,
                Price = req.Price,
                Quantity = req.Quantity,
                CreatedBy = req.CreatedBy
            });
            return result;
        }

        public async Task<List<ProductResponse>> GetAllProductsAsync()
        {
            var result = await GetDataAsync<ProductResponse>("SP_GetAllProducts");
            return result.ToList();
        }

        public async Task<ProductResponse?> GetProductByIdAsync(int id)
        {
            var result = await GetDataAsync<ProductResponse>("SP_GetProductById", new { Id = id });
            return result.FirstOrDefault();
        }

        public async Task<int> UpdateProductAsync(ProductRequest req)
        {
            var result = await ExecuteDataAsync("SP_UpdateProduct", new
            {
                Id = req.Id,
                ProductName = req.ProductName,
                Description = req.Description,
                Price = req.Price,
                Quantity = req.Quantity,
                UpdatedBy = Environment.UserName
            });
            return result;
        }

        public async Task<int> DeleteProductAsync(int id)
        {
            var result = await ExecuteDataAsync("SP_DeleteProduct", new
            {
                Id = id
            });
            return result;
        }
    }
}