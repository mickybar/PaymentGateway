using PaymentGateway.Data.Entities;

namespace PaymentGateway.Api.Services
{
    public interface ICardService
    { 
        Task<Card?> GetCardByPan(string pan);
        Task<Card> AddCard(Card cardDetails);
    }
}
