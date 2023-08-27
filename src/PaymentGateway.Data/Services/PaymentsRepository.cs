using Microsoft.EntityFrameworkCore;
using PaymentGateway.Data.DbContexts;
using PaymentGateway.Data.Entities;

namespace PaymentGateway.Data.Services
{
    public class PaymentsRepository : IPaymentsRepository
    {
        private readonly PaymentsContext _context;

        public PaymentsRepository(PaymentsContext context)
        {
            _context = context;
        }
        public async Task<Payment?> GetPaymentByPaymentId(string paymentId)
        {
            return await _context.Payments.Include(c => c.CardDetails)
                .Where(c => string.Equals(c.PaymentId,paymentId))
                .FirstOrDefaultAsync();
        }

        public async Task<Payment> AddPayment(Payment payment)
        {
            _context.Payments.Add(payment);
            await SaveChanges();
            return payment;
        }

        public async Task<Payment> UpdatePayment(Payment payment)
        {
            var paymentToUpdate = await _context.Payments.Include(c => c.CardDetails)
                .Where(c => string.Equals(c.PaymentId, payment.PaymentId))
                .FirstAsync();

            paymentToUpdate.Status = payment.Status;
            paymentToUpdate.LastUpdated = DateTime.Now;
            await SaveChanges();
            return paymentToUpdate;
        }

        public Task<Card?> GetCardDetailsByPan(string pan)
        {
            return _context.CardDetails.Where(c => c.Pan == pan).FirstOrDefaultAsync();
        }

        public async Task<Card> AddCard(Card cardDetails)
        {
            _context.CardDetails.Add(cardDetails);
            await SaveChanges();
            return cardDetails;
        }

        private async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
