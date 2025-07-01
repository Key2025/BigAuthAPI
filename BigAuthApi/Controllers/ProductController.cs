using BigAuthApi.Model.Request;
using BigAuthApi.Model.Response;
using BigAuthApi.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BigAuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ADMIN, USER")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("get-all-products")]
        public async Task<ActionResult<ProductResponse>> GetAllProductsAsync()
        {
            var result = await _productService.GetAllProductsAsync();
            return Ok(result);
        }

        [HttpPost("add-product")]
        public async Task<ActionResult<int>> AddProductAsync([FromBody] ProductRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);  // Will auto-check annotations
            var result = await _productService.AddProductAsync(req);

            if (result > 0)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error adding product.");
            }
        }
    }
}