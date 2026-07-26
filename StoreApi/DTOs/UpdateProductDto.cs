using System.ComponentModel.DataAnnotations;

namespace StoreApi.DTOs
{
    public class UpdateProductDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Category { get; set; } = string.Empty;

        [Range(1, 1000000000)]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        [Range(0, 100000)]
        public int Stock { get; set; }
    }
}
