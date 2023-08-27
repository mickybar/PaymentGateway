using PaymentGateway.Data.Entities;
using PaymentGateway.Data.Services;

namespace PaymentGateway.Api.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentsRepository _paymentRepository;

        public PaymentService(IPaymentsRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<Payment?> GetPaymentById(string id)
        {
            return await _paymentRepository.GetPaymentByPaymentId(id);
        }

        public async Task<Payment> SavePayment(Payment payment)
        {
            return await _paymentRepository.AddPayment(payment);
        }

        public async Task<Payment> UpdatePayment(Payment payment)
        {
            return await _paymentRepository.UpdatePayment(payment);
        }
    }
}
