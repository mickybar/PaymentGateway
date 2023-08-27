using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Common.Enum;
using PaymentGateway.Common.Models;

namespace CKOBank.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizationController : ControllerBase
    {
        [HttpPost]
        public ActionResult<AuthorizationResponse> Authorize(PaymentRequestDto paymentRequest)
        {
            var response = new AuthorizationResponse();

            switch (paymentRequest.Amount)
            {
                case 60:
                    response.Status = Status.CardVerified.ToString();
                    break;
                case 70:
                    response.Status = Status.Declined.ToString();
                    break;
                case 80:
                    response.Status = Status.Pending.ToString();
                    break;
                default:
                    response.Status = Status.Authorized.ToString();
                    break;
            }

            return Ok(response);
        }
    }
}
