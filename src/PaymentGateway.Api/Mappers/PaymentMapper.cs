using PaymentGateway.Api.Models;
using PaymentGateway.Common.Models;
using PaymentGateway.Data.Entities;

namespace PaymentGateway.Api.Mappers
{
    public class PaymentMapper : IPaymentMapper
    {
        public Payment MapToPaymentEntity(PaymentRequestDto paymentRequestDto, Card card, string status)
        {
            return new Payment()
            {
                PaymentId = Guid.NewGuid().ToString(),
                Amount = paymentRequestDto.Amount,
                Currency = paymentRequestDto.Currency,
                Reference = paymentRequestDto.Reference,
                CardId = card.Id,
                CardDetails = card,
                Created = DateTime.Now,
                LastUpdated = DateTime.Now,
                Status = status
            };
        }

        public PaymentResponseDto MapPaymentEntityToPaymentResponseDto(Payment payment, CardDetails cardDetails)
        {
            return new PaymentResponseDto()
            {
                Id = payment.PaymentId,
                Amount = payment.Amount,
                Currency = payment.Currency,
                Reference = payment.Reference,
                Status = payment.Status,
                CardDetails = cardDetails
            };
        }
    }
}
