using PaymentGateway.Common.Models;
using PaymentGateway.Data.Entities;

namespace PaymentGateway.Api.Mappers;

public interface ICardMapper
{
    Card MapCardDetailsToCardEntity(CardDetails cardDetails);
    CardDetails MapCardEntityToCardDetails(Card card);
}