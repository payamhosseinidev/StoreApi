namespace StoreApi.Responses
{
    public class ValidationErrorResponse
    {
        public int Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public Dictionary<string, string[]> Errors { get; set; } = new();
    }
}
