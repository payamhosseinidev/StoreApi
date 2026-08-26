using StoreApi.Models;
using System.ComponentModel.DataAnnotations;

namespace StoreApi.DTOs
{
    public class CreateOrderDto
    {
        public List<CreateOrderItemDto> Items { get; set; } = new();
    }
}
