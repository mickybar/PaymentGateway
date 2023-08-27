using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Common.Models
{
    public class PaymentRequestDto
    {
        [Required]
        public decimal Amount { get; set; }

        [Required] public string Currency { get; set; } = null!;
        [Required]
        public string Reference { get; set; } = null!;

        [Required] public CardDetails CardDetails { get; set; } = null!;

    }
}
