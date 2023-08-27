using Microsoft.EntityFrameworkCore;
using PaymentGateway.Data.Entities;

namespace PaymentGateway.Data.DbContexts
{
    public class PaymentsContext : DbContext
    {
        public DbSet<Payment> Payments { get; set; } = null!;
        public DbSet<Card> CardDetails { get; set; } = null!;

        public PaymentsContext(DbContextOptions<PaymentsContext> options) : base(options)
        {
            
        }
    }
}
