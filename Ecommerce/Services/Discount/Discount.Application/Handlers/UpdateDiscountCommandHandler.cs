using AutoMapper;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Discount.Grpc.Proto;
using MediatR;

namespace Discount.Application.Commands
{
    public class UpdateDiscountCommandHandler(IDiscountRepository discountRepository, IMapper mapper) : IRequestHandler<UpdateDiscountCommand, CouponModel>
    {
        private readonly IDiscountRepository _discountRepository = discountRepository;
        public readonly IMapper _mapper = mapper;

        public async Task<CouponModel> Handle(UpdateDiscountCommand request, CancellationToken cancellationToken)
        {
            var coupon = _mapper.Map<Coupon>(request);
            await _discountRepository.UpdateDiscount(coupon);
            var couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;
        }
    }
}
