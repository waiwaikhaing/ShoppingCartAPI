using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartAPI.EFDBContext;
using ShoppingCartAPI.Helper;
using ShoppingCartAPI.Models;
using ShoppingCartAPI.Services;

namespace ShoppingCartAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly ILogger<ShoppingCartController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IShoppingCartService _cartService;
        public ShoppingCartController(
            ILogger<ShoppingCartController> logger,
        IHttpContextAccessor httpContextAccessor, IShoppingCartService cartService)
        {
            _cartService = cartService;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        }

        [HttpGet("GetCartItem")]
        public async Task<IActionResult> GetCartItem()
        {
            var userId = GetUserId();
            var result = await _cartService.GetCartItemsAsync(userId);
            return Ok(result);
        }

        [HttpPost("AddCartItem")]
        public async Task<IActionResult> AddCartItem([FromBody] List<CartItemDTO> itemDto)
        {
            var userId = GetUserId();
            var result = await _cartService.AddItemToCartAsync(userId, itemDto);
            return Ok(result);
        }

        [HttpDelete("remove/{id}")]
        public async Task<IActionResult> RemoveItem(string id)
        {
            var result = await _cartService.RemoveItemFromCartAsync(id);
            return Ok(result);
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout()
        {
            var userId = GetUserId();
            var result = await _cartService.CheckoutAsync(userId);
            return Ok(result);
        }
    }

}
