using Discount.Application.Mapper;
using Discount.Application.Queries;
using Discount.Core.Repositories;
using Discount.Grpc.Proto;
using Grpc.Core;
using MediatR;

namespace Discount.Application.Handlers
{
    class GetDiscountQueryHandler(IDiscountRepository discountRepository) : IRequestHandler<GetDiscountQuery, CouponModel>
    {
        private readonly IDiscountRepository _discountRepository = discountRepository;

        public async Task<CouponModel> Handle(GetDiscountQuery request, CancellationToken cancellationToken)
        {
            var coupon = await _discountRepository.GetDiscount(request.ProductName);
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount for Product Name = {request.ProductName} not found"));
            }
            var couponModel = new CouponModel
            {
                Id = coupon.Id,
                ProductName = coupon.ProductName,
                Amount = coupon.Amount,
                Description = coupon.Description
            };
            return couponModel;
        }
    }
}
