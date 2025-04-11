using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCartAPI.Models
{
    [Table("CartItem")]
    public class CartItem
    {
        [Key]
        public string CartItemId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string ProductId { get; set; }
        public decimal Price { get; set; }
        [Required]
        public decimal? Qty { get; set; }
        public string Status { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool Active { get; set; }
    }

    public class CartItemDTO
    {
        [Required]
        public string ProductId { get; set; }
        
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Quantity must be a positive number.")]

        public decimal? Qty { get; set; }
    }

    public class CartItemDetailDTO
    {
        public string CartItemId { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal? Qty { get; set; }
        public decimal? Price { get; set; }
        public string Status { get; set; }
    }

    public class CartItemResponselDTO
    {
        public string CartItemId { get; set; }
        public string ProductName { get; set; }
        public string Message { get; set; }
    }
}
