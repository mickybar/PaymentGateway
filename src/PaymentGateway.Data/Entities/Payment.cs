using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Data.Entities
{
    public class Payment
    {
        [Key] [Required] public string PaymentId { get; set; } = null!;
        public decimal Amount { get; set; }
        public int CardId { get; set; }
        [Required]
        public Card CardDetails { get; set; } = null!;
        [Required] 
        public string Currency { get; set; } = null!;
        [Required]
        public string Reference { get; set; } = null!;
        [Required]
        public string Status { get; set; } = null!;
        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
