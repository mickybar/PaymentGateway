using PaymentGateway.Common.Models;

namespace PaymentGateway.Api.Services
{
    public interface ICKOBankService
    {
        Task<AuthorizationResponse> AuthorizePayment(PaymentRequestDto paymentRequest);
    }
}
