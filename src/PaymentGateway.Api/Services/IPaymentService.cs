using PaymentGateway.Data.Entities;

namespace PaymentGateway.Api.Services
{
    public interface IPaymentService
    {
        Task<Payment?> GetPaymentById(string id);
        Task<Payment> SavePayment(Payment payment);
        Task<Payment> UpdatePayment(Payment payment);
    }
}
