using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCartAPI.Models
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public string ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public decimal? Qty { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool Active { get; set; }
    }

    public class ProductDTO
    {
        [Required]
        [MaxLength(50, ErrorMessage = "ProductName cannot exceed 50 characters.")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "ProductName must be between 10 and 50 characters.")]

        public string ProductName { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Unit Price must be a positive number.")]
        public decimal UnitPrice { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Quantity must be a positive number.")]
        public decimal? Qty { get; set; }
    }

}
