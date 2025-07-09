using Common.Logging.Correlation;
using Discount.Application.Commands;
using Discount.Application.Queries;
using Discount.Grpc.Proto;
using Grpc.Core;
using MediatR;

namespace Discount.API.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<DiscountService> _logger;
        private readonly ICorrelationIDGenerator _correlationIDGenerator;

        public DiscountService(IMediator mediator, ILogger<DiscountService> logger, ICorrelationIDGenerator correlationIDGenerator)
        {
            _mediator = mediator;
            _logger = logger;
            _correlationIDGenerator = correlationIDGenerator;
            _logger.LogInformation("Correlation Id {correlationId}", _correlationIDGenerator.Get());
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var query = new GetDiscountQuery(request.ProductName);
            var result = await _mediator.Send(query);
            return result;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var cmd = new CreateDiscountCommand
            {
                ProductName = request.Coupon.ProductName,
                Amount = request.Coupon.Amount,
                Description = request.Coupon.Description
            };
            var result = await _mediator.Send(cmd);
            return result;
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var cmd = new UpdateDiscountCommand
            {
                Id = request.Coupon.Id,
                ProductName = request.Coupon.ProductName,
                Amount = request.Coupon.Amount,
                Description = request.Coupon.Description
            };
            var result = await _mediator.Send(cmd);
            return result;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var cmd = new DeleteDiscountCommand(request.ProductName);
            var deleted = await _mediator.Send(cmd);
            var response = new DeleteDiscountResponse
            {
                Success = deleted
            };
            return response;
        }
    }
}
