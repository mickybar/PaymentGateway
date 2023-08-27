using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PaymentGateway.Api.Controllers;
using PaymentGateway.Api.Mappers;
using PaymentGateway.Api.Models;
using PaymentGateway.Api.Services;
using PaymentGateway.Common.Enum;
using PaymentGateway.Common.Models;
using PaymentGateway.Data.Entities;
using Xunit;

namespace PaymentGateway.Api.Tests
{
    public class PaymentControllerShould
    {
        private readonly Mock<ICardService> _cardService;
        private readonly Mock<IPaymentService> _paymentService;
        private readonly Mock<ICardMapper> _cardMapper;
        private readonly Mock<IPaymentMapper> _paymentMapper;
        private readonly Mock<ICKOBankService> _ckoBankService;
        private readonly Mock<ILogger<PaymentsController>> _logger;

        private readonly PaymentsController _paymentController;

        public PaymentControllerShould()
        {

            _cardService = new Mock<ICardService>();
            _paymentService = new Mock<IPaymentService>();
            _cardMapper = new Mock<ICardMapper>();
            _paymentMapper = new Mock<IPaymentMapper>();
            _ckoBankService = new Mock<ICKOBankService>();
            _logger = new Mock<ILogger<PaymentsController>>();

            _paymentController = new PaymentsController(_cardService.Object,
                _paymentService.Object,
                _cardMapper.Object,
                _paymentMapper.Object,
                _ckoBankService.Object,
                _logger.Object);
        }

        [Fact]
        public async Task ReturnAuthorizedPaymentId_WhenPostingAPayment_And_PaymentIsAuthorized()
        {
            var finalStatus = Status.Authorized.ToString();
            var paymentRequest = Helper.GetValidPaymentRequestDto();

            SetUpDefaultMocks(paymentRequest,finalStatus);

            var actionResult = await _paymentController.ProcessPayment(paymentRequest);

            var okObjectResult = actionResult.Result as OkObjectResult;
            var result = okObjectResult?.Value as AuthorizationResponseDto;

            Assert.NotNull(result);
            Assert.Equal(Helper.PaymentId, result.PaymentId);
            Assert.Equal(finalStatus, result.Status);
        }

        [Fact]
        public async Task SaveTheCard_WhenPostingAPayment_And_TheCardIsNew()
        {
            var finalStatus = Status.Authorized.ToString();
            var paymentRequest = Helper.GetValidPaymentRequestDto();

            SetUpDefaultMocks(paymentRequest, finalStatus);
            _cardService.Setup(c => c.GetCardByPan(It.IsAny<string>())).ReturnsAsync(() => null);

            var result = await _paymentController.ProcessPayment(paymentRequest);

            _cardService.Verify(c => c.AddCard(It.IsAny<Card>()), Times.Once);
        }

        [Fact]
        public async Task ReturnBadRequest_WhenPostingAPayment_And_CardDataIsInvalid()
        {
            var paymentRequest = Helper.GetPaymentRequestDtoWithInvalidCardData();

            var actionResult = await _paymentController.ProcessPayment(paymentRequest);

            var badRequestObjectResult = actionResult?.Result as BadRequestObjectResult;

            Assert.NotNull(badRequestObjectResult);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
        }

        [Fact]
        public async Task ReturnBadRequest_WhenPostingAPayment_And_PaymentRequestDtoIsIncomplete()
        {
            var paymentRequest = Helper.GetInvalidPaymentRequestDto();

            var actionResult = await _paymentController.ProcessPayment(paymentRequest);

            var badRequestObjectResult = actionResult?.Result as BadRequestObjectResult;

            Assert.NotNull(badRequestObjectResult);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
        }

        [Fact]
        public async Task ReturnInternalServerError_WhenPostingAPayment_And_ExceptionRaised()
        {
            var paymentRequest = Helper.GetValidPaymentRequestDto();
            _cardService.Setup(c => c.GetCardByPan(It.IsAny<string>())).ThrowsAsync(new Exception());

            var actionResult = await  _paymentController.ProcessPayment(paymentRequest);
            var objectResult = actionResult.Result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async Task WriteLog_WhenPostingAPayment_And_ExceptionRaised()
        {
            var paymentRequest = Helper.GetValidPaymentRequestDto();
            _cardService.Setup(c => c.GetCardByPan(It.IsAny<string>())).ThrowsAsync(new Exception());

            await _paymentController.ProcessPayment(paymentRequest);

            _logger.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)));
        }

        [Fact]
        public async Task ReturnAPayment_WhenGettingAPayment_And_PaymentExists()
        {
            var cardDetails = Helper.GetMaskedCardDetails();

            _paymentService.Setup(p => p.GetPaymentById(It.IsAny<string>())).ReturnsAsync(() => new Payment());

            _cardMapper.Setup(c => c.MapCardEntityToCardDetails(It.IsAny<Card>())).Returns(cardDetails);

            _paymentMapper.Setup(p =>
                    p.MapPaymentEntityToPaymentResponseDto(It.IsAny<Payment>(), It.IsAny<CardDetails>()))
                .Returns(new PaymentResponseDto()
                {
                    Status =Status.Authorized.ToString(),
                    CardDetails = cardDetails,
                    Id = Helper.PaymentId
                });

            var actionResult = await _paymentController.GetPayment(Helper.PaymentId);
            var okObjectResult = actionResult.Result as OkObjectResult;
            var result = okObjectResult?.Value as PaymentResponseDto;

            Assert.NotNull(okObjectResult);
            Assert.NotNull(result);
            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.Equal(Helper.PaymentId, result.Id);
            Assert.Equal(Helper.MaskedPan, result.CardDetails.Pan);
        }

        [Fact]
        public async Task ReturnNotFound_WhenGettingAPayment_And_PaymentDoesNotExist()
        {
            _paymentService.Setup(p => p.GetPaymentById(It.IsAny<string>())).ReturnsAsync(() => null);

            var actionResult = await _paymentController.GetPayment(Helper.PaymentId);
            var notFoundObjectResult = actionResult.Result as NotFoundObjectResult;

            Assert.NotNull(notFoundObjectResult);
            Assert.Equal(404, notFoundObjectResult.StatusCode);
        }

        [Fact]
        public async Task ReturnInternalServerError_WhenGettingAPayment_And_ExceptionRaised()
        {

            _paymentService.Setup(p => p.GetPaymentById(It.IsAny<string>())).ThrowsAsync(new Exception());

            var actionResult = await _paymentController.GetPayment(Helper.PaymentId);
            var objectResult = actionResult.Result as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async Task WriteLog_WhenGettingAPayment_And_ExceptionRaised()
        {

            _paymentService.Setup(p => p.GetPaymentById(It.IsAny<string>())).ThrowsAsync(new Exception());

            var actionResult = await _paymentController.GetPayment(Helper.PaymentId);

            _logger.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)));
        }

        private void SetUpDefaultMocks(PaymentRequestDto paymentRequest, string finalStatus)
        {
            _cardService.Setup(c => c.GetCardByPan(It.IsAny<string>())).ReturnsAsync(() => new Card());

            _paymentMapper.Setup(c => c.MapToPaymentEntity(paymentRequest, It.IsAny<Card>(), Status.Created.ToString()))
                .Returns(() => new Payment());

            _paymentService.Setup(c => c.SavePayment(It.IsAny<Payment>())).ReturnsAsync(new Payment()
            {
                PaymentId = Helper.PaymentId,
                Status = Status.Created.ToString()
            });

            _ckoBankService.Setup(c => c.AuthorizePayment(paymentRequest)).ReturnsAsync(() => new AuthorizationResponse()
            {
                Status = finalStatus
            });

            _paymentService.Setup(c => c.UpdatePayment(It.IsAny<Payment>())).ReturnsAsync(new Payment()
            {
                PaymentId = Helper.PaymentId,
                Status = finalStatus
            });
        }

    }
}
