namespace Academy.Models
{
    public enum OrderStatus
    {
        Pending = 0,
        Paid = 1,
        Failed = 2,
        Refunded = 3
    }

    public class Order : BaseEntity
    {
        public string AppUserId { get; set; } = null!;
        public AppUser AppUser { get; set; } = null!;

        public string OrderNumber { get; set; } = null!;

        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;

        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; } = "card";
        public string? StripePaymentIntentId { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
