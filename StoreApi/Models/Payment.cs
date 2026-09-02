namespace StoreApi.Models
{
    public class Payment
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public decimal Amount { get; set; }

        public PaymentStatus Status { get; set; }

        public string? TransactionId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
