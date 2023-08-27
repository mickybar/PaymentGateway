using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Api.Extensions;
using PaymentGateway.Api.Mappers;
using PaymentGateway.Api.Models;
using PaymentGateway.Api.Services;
using PaymentGateway.Api.Validation;
using PaymentGateway.Common.Enum;
using PaymentGateway.Common.Models;

namespace PaymentGateway.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly ICardService _cardService;
        private readonly IPaymentService _paymentService;
        private readonly ICardMapper _cardMapper;
        private readonly IPaymentMapper _paymentMapper;
        private readonly ICKOBankService _ckoBankService;
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(ICardService cardService, 
            IPaymentService paymentService,
            ICardMapper cardMapper,
            IPaymentMapper paymentMapper,
            ICKOBankService ckoBankService,
            ILogger<PaymentsController> logger)
        {
            _cardService = cardService;
            _paymentService = paymentService;
            _cardMapper = cardMapper;
            _paymentMapper = paymentMapper;
            _ckoBankService = ckoBankService;
            _logger = logger;
        }

        [HttpPost]
        public async Task <ActionResult<AuthorizationResponseDto>> ProcessPayment(PaymentRequestDto paymentRequestDto)
        {
            if (!CardValidator.IsValid(paymentRequestDto.CardDetails.Pan,
                    paymentRequestDto.CardDetails.ExpiryMonth,
                    paymentRequestDto.CardDetails.ExpiryYear,
                    paymentRequestDto.CardDetails.Cvv))
            {
                return BadRequest("Card data is invalid");
            }

            try
            {
                var card = await _cardService.GetCardByPan(paymentRequestDto.CardDetails.Pan);
                if (card == null)
                {
                    card = _cardMapper.MapCardDetailsToCardEntity(paymentRequestDto.CardDetails);
                    await _cardService.AddCard(card);
                }

                var paymentToProcess = _paymentMapper.MapToPaymentEntity(paymentRequestDto, card, Status.Created.ToString());
                var savedPayment = await _paymentService.SavePayment(paymentToProcess);

                var result = await _ckoBankService.AuthorizePayment(paymentRequestDto);
                savedPayment.Status = result.Status;

                
                var updatedPayment = await _paymentService.UpdatePayment(savedPayment);
                return Ok(new AuthorizationResponseDto()
                {
                    PaymentId = updatedPayment.PaymentId,
                    Status = updatedPayment.Status
                });

            } 
            catch (Exception e)
            {
                _logger.LogError($"Exception while processing payment {paymentRequestDto.Reference}", e);
                return StatusCode(500, "An error occurred while processing your payment. Please try again");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentResponseDto>> GetPayment(string id)
        {
            try
            {
                var payment = await _paymentService.GetPaymentById(id.ToLowerInvariant());
                if (payment == null)
                {
                    return NotFound("Payment not found");
                }

                var cardDetails = _cardMapper.MapCardEntityToCardDetails(payment.CardDetails);
                cardDetails.Pan = cardDetails.Pan.Mask(6, 8);

                return Ok(_paymentMapper.MapPaymentEntityToPaymentResponseDto(payment, cardDetails));

            }
            catch (Exception e)
            {
                _logger.LogError($"Exception while processing payment {id}", e);
                return StatusCode(500, "An error occurred while retrieving your payment. Please try again");
            }
        }
    }
}