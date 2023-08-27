using PaymentGateway.Api.Models;
using PaymentGateway.Common.Models;
using PaymentGateway.Data.Entities;

namespace PaymentGateway.Api.Mappers;

public interface IPaymentMapper
{
    Payment MapToPaymentEntity(PaymentRequestDto paymentRequestDto, Card card, string status);
    PaymentResponseDto MapPaymentEntityToPaymentResponseDto(Payment payment, CardDetails cardDetails);
}