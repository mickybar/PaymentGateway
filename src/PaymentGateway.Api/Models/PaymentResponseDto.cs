using PaymentGateway.Common.Models;

namespace PaymentGateway.Api.Models
{
    public class PaymentResponseDto
    {
        public string? Id { get; set; }
        public decimal Amount { get; set; }
        public string? Status { get; set; }
        public string? Currency { get; set; }
        public string? Reference { get; set; }
        public CardDetails? CardDetails { get; set; }
    }
}
