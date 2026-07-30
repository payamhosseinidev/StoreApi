using System.ComponentModel.DataAnnotations;

namespace StoreApi.DTOs
{
    public class CreateProductDto
    {
        [Display(Name = "نام محصول")]
        [Required(ErrorMessage = "{0} الزامی است.")]
        [MinLength(3)]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Category { get; set; } = string.Empty;

        [Display(Name = "قیمت")]
        [Range(1, 1000000000,
            ErrorMessage = "{0} باید بین {1} و {2} باشد.")]
        public decimal Price { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        [Range(0, 100000)]
        public int Stock { get; set; }

    }
}
