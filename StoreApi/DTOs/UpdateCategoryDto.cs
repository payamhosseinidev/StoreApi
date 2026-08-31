using System.ComponentModel.DataAnnotations;

namespace StoreApi.DTOs
{
    public class UpdateCategoryDto
    {
        [Required(ErrorMessage = "نام دسته‌بندی الزامی است.")]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }
    }
}
