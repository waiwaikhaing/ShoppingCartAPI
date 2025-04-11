using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartAPI.Models;
using ShoppingCartAPI.Services;

namespace ShoppingCartAPI.Controllers
{
   // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }


        [AllowAnonymous]
        [HttpGet("GetProducts")]
        public async Task<IActionResult> GetProducts()
        {
            var result = await _productService.GetProducts();
            return Ok(result);
        }

        [HttpPost("AddProducts")]
        public async Task<IActionResult> AddProducts([FromBody] List<ProductDTO> itemDto)
        {
            var result = await _productService.AddProduct(itemDto);
            return Ok(result);
        }
    }
}
