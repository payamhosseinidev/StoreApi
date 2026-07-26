using System.ComponentModel.DataAnnotations;

namespace StoreApi.Models
{
    public class Product
    {
       public int Id { get; set; }

       [Required]
       [StringLength(100)]
       public string Name { get; set; } = string.Empty;

       [Required]
       [StringLength(500)]
       public string Description { get; set; } = string.Empty;

       [Required]
       public string Category { get; set; } =  string.Empty;

       [Range(1, 1000000000)]
       public decimal Price { get; set; }

       public string ImageUrl { get; set; } = string.Empty;

       [Range(0, 100000)]
       public int Stock { get; set; }

       public DateTime CreatedAt { get; set; }
    }
}
