using System.ComponentModel.DataAnnotations;

namespace StoreApi.DTOs
{
    public class CreateOrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
