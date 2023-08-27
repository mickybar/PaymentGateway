using System.Text;
using System.Text.Json;
using PaymentGateway.Common.Models;

namespace PaymentGateway.Api.Services
{
    public class CKOBankService : ICKOBankService
    {
        private readonly HttpClient _ckoBankHttpClient;

        public CKOBankService(IHttpClientFactory factory)
        {
            _ckoBankHttpClient = factory.CreateClient("CKOBankClient");
        }

        public async Task<AuthorizationResponse> AuthorizePayment(PaymentRequestDto paymentRequest)
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(paymentRequest),Encoding.UTF8,"application/json");
            using var response = await _ckoBankHttpClient.PostAsync("authorization", jsonContent);

            response.EnsureSuccessStatusCode();

            var status = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var paymentAuthorizationOutcome = JsonSerializer.Deserialize<AuthorizationResponse>(status, options);

            return paymentAuthorizationOutcome;
        }
    }
}
