using Basket.Application.Commands;
using Basket.Application.Mappers;
using Basket.Application.Responses;
using Basket.Core.Repositories;
using MediatR;
using Basket.Core.Entities;
using Basket.Application.GrpcService;

namespace Basket.Application.Hadlers
{
    public class CreateShoppingCartHandler(IBasketRepository basketRepository, DiscountGrpService discountGrpService) : IRequestHandler<CreateShoppingCartCommand, ShoppingCartResponse>
    {
        private readonly IBasketRepository _basketRepository = basketRepository;
        private readonly DiscountGrpService _discountGrpService = discountGrpService;

        public async Task<ShoppingCartResponse> Handle(CreateShoppingCartCommand request, CancellationToken cancellationToken)
        {
            foreach (var item in request.Items)
            {
                var coupon = await _discountGrpService.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount;
            }

            var shoppingCart = await _basketRepository.UpdateBasket(new ShoppingCart
            {
                UserName = request.UserName,
                Items = request.Items
            });

            var shoppingCartResponse = BasketMapper.Mapper.Map<ShoppingCartResponse>(shoppingCart);
            return shoppingCartResponse;
        }
    }
}
