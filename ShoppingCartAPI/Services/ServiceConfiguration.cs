namespace ShoppingCartAPI.Services
{
    public class ServiceConfiguration
    {
        public void AddServices(IServiceCollection services)
        {
            services.AddScoped<IShoppingCartService, ShoppingCartService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProductService, ProductService>();
        }
    }
}
