using PaymentGateway.Data.Entities;
using PaymentGateway.Data.Services;

namespace PaymentGateway.Api.Services
{
    public class CardService : ICardService
    {
        private readonly IPaymentsRepository _paymentRepository;

        public CardService(IPaymentsRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<Card?> GetCardByPan(string pan)
        {
            return await _paymentRepository.GetCardDetailsByPan(pan);
        }

        public async Task<Card> AddCard(Card cardDetails)
        {
            return await _paymentRepository.AddCard(cardDetails);
        }
    }
}
