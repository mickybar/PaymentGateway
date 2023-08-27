using PaymentGateway.Common.Models;
using PaymentGateway.Data.Entities;

namespace PaymentGateway.Api.Tests
{
    public static class Helper
    {
        public const string PaymentId = "7dc35b8c-d764-4f45-8dae-b4ff3319742c";
        public const string MaskedPan = "424242XXXXXXXX42";
        public static PaymentRequestDto GetValidPaymentRequestDto()
        {
            return new PaymentRequestDto()
            {
                Amount = 10,
                Currency = "EUR",
                Reference = "test1-ref",
                CardDetails = new CardDetails
                {
                    Pan = "4242424242424242",
                    ExpiryMonth = "12",
                    ExpiryYear = "25",
                    Cvv = "123"
                }
            };
        }

        public static PaymentRequestDto GetPaymentRequestDtoWithInvalidCardData()
        {
            return new PaymentRequestDto()
            {
                Amount = 10,
                Currency = "EUR",
                Reference = "test1-ref",
                CardDetails = new CardDetails
                {
                    Pan = "123456789123456",
                    ExpiryMonth = "12",
                    ExpiryYear = "25",
                    Cvv = "123"
                }
            };
        }

        public static PaymentRequestDto GetInvalidPaymentRequestDto()
        {
            return new PaymentRequestDto()
            {
                Amount = 10,
                CardDetails = new CardDetails
                {
                    Pan = "123456789123456",
                    ExpiryMonth = "12",
                    ExpiryYear = "25",
                    Cvv = "123"
                }
            };
        }

        public static CardDetails GetUnmaskedCardDetails()
        {
            return new CardDetails()
            {
                Pan = "4242424242424242",
                Cvv = "123",
                ExpiryMonth = "12",
                ExpiryYear = "25"
            };
        }

        public static CardDetails GetMaskedCardDetails()
        {
            return new CardDetails()
            {
                Pan = "424242XXXXXXXX42",
                Cvv = "123",
                ExpiryMonth = "12",
                ExpiryYear = "25"
            };
        }
    }
}

