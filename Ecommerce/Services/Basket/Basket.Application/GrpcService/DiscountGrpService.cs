using Discount.Grpc.Proto;

namespace Basket.Application.GrpcService
{
    public class DiscountGrpService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;

        public DiscountGrpService(DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
        {
            _discountProtoServiceClient = discountProtoServiceClient;
        }

        public async Task<CouponModel> GetDiscount(string productName)
        {
            var discountRequest = new GetDiscountRequest { ProductName = productName };
            Console.WriteLine("VALEUR DE LAVARIABLE discountRequest: " + discountRequest.ProductName);
            return await _discountProtoServiceClient.GetDiscountAsync(discountRequest);
        }
    }
}
