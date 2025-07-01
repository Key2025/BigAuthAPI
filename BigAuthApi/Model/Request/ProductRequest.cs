using System.ComponentModel.DataAnnotations;

namespace BigAuthApi.Model.Request
{
    public class ProductRequest
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string ProductName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;

        [Range(0.01, 10000)]
        public decimal Price { get; set; }

        [Display(Name = "In Stock")]
        public int Quantity { get; set; }

        public string CreatedBy { get; set; } = string.Empty;
    }
}