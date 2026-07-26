using System.ComponentModel.DataAnnotations;

namespace StoreApi.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public int Stock { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
