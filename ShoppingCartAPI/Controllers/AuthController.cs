using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCartAPI.EFDBContext;
using ShoppingCartAPI.Models;
using ShoppingCartAPI.Services;

namespace ShoppingCartAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController( IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDTO dto)
        {
            //register user
            var result = await _userService.UserRegister(dto);
            if (!result.Success)
            {
                return BadRequest(result);  
            }

            return Ok(result);  
        }

        [HttpPost("GenerateToken")]
        public async Task<IActionResult> GenerateToken(UserDTO dto)
        {
            //generate token
            var result = await _userService.GenerateToken(dto);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

    }

}
