using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using ShoppingCartAPI.EFDBContext;
using ShoppingCartAPI.Models;

namespace ShoppingCartAPI.Services
{
    public interface IUserService
    {
        Task<ServiceResponse> UserRegister(UserDTO dto);
        Task<TokenResponse> GenerateToken(UserDTO dto);
    }

        public class UserService: IUserService
        {
        private readonly AppDBContext _context;
        private readonly IJwtService _jwtService;
        public UserService(AppDBContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<ServiceResponse> UserRegister(UserDTO dto)
        {
            var userData = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (userData != null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Username already exists"
                };
            }

            var user = new UserData
            {
                UserId = Guid.NewGuid().ToString(),
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Active = true,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = Guid.NewGuid().ToString()
            };


            _context.Users.Add(user);
            var result = await _context.SaveChangesAsync();
            
            return new ServiceResponse
            {
                Success = true,
                Message = "Registration successful"
            };
        }

        public async Task<TokenResponse> GenerateToken(UserDTO dto)
        {
            // Validate user credentials
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                return new TokenResponse
                {
                    Success = false,
                    Message = "Invalid username or password"
                };
            }
            var secureKey = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            Console.WriteLine(secureKey);

            // Generate JWT token
            var token = await _jwtService.GenerateToken(user);

            return new TokenResponse
            {
                Success = true,
                Message = "Token generated successfully",
                Token = token
            };
        }

    }
}
