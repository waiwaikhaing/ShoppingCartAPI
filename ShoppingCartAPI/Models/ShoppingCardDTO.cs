using System.ComponentModel.DataAnnotations;

namespace ShoppingCartAPI.Models
{
    public class ServiceResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class ServiceDataResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }

    public class TokenResponse : ServiceResponse
    {
        public string Token { get; set; }
    }

}
