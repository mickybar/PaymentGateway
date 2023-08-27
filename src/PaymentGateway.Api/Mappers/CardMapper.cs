using PaymentGateway.Common.Models;
using PaymentGateway.Data.Entities;

namespace PaymentGateway.Api.Mappers
{
    public class CardMapper : ICardMapper
    {
        public Card MapCardDetailsToCardEntity(CardDetails cardDetails)
        {
            return new Card()
            {
                Pan = cardDetails.Pan,
                Cvv = cardDetails.Cvv,
                ExpiryMonth = cardDetails.ExpiryMonth,
                ExpiryYear = cardDetails.ExpiryYear
            };
        }

        public CardDetails MapCardEntityToCardDetails(Card card)
        {
            return new CardDetails()
            {
                Pan = card.Pan,
                Cvv = card.Cvv,
                ExpiryMonth = card.ExpiryMonth,
                ExpiryYear = card.ExpiryYear
            };
        }
    }
}
