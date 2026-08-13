namespace StoreApi.DTOs
{
    public class ProductFilterDto
    {
        public string? Category { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; }
        public string? Search { get; set; }
    }
}
