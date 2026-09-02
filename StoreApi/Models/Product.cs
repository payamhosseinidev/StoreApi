using System.ComponentModel.DataAnnotations;

namespace StoreApi.Models
{
    public class Product
    {
       public int Id { get; set; }
       public string Name { get; set; } = string.Empty;
       public string Description { get; set; } = string.Empty;

       public int? CategoryId { get; set; }

       public Category? Category { get; set; } = null!;
       public decimal Price { get; set; }

       public string ImageUrl { get; set; } = string.Empty;

       public int Stock { get; set; }

       public DateTime CreatedAt { get; set; }

       public ICollection<OrderItem> OrdersItems { get; set; } = new List<OrderItem>();

       public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }
}
