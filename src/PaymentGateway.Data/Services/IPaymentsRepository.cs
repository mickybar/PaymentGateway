using PaymentGateway.Data.Entities;

namespace PaymentGateway.Data.Services
{
    public interface IPaymentsRepository
    {
        Task<Payment?> GetPaymentByPaymentId(string paymentId);
        Task<Payment> AddPayment(Payment payment);
        Task<Payment> UpdatePayment(Payment payment);
        Task<Card?> GetCardDetailsByPan(string pan);
        Task<Card> AddCard(Card cardDetails);
    }
}
