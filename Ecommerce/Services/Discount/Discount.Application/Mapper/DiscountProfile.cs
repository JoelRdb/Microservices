using AutoMapper;
using Discount.Core.Entities;
using Discount.Grpc.Proto;

namespace Discount.Application.Mapper
{
    public class DiscountProfile : Profile
    {
        public DiscountProfile()
        {
            //Change property of discount.poto like this : (Build Action : Protobuf compiler ; gRPC Stub Classes : Server only).
            // CouponModel created automatiacally after buil Discount.Application by gRPC (discount.proto) 
            CreateMap<Coupon, CouponModel>().ReverseMap();
        }
    }
}
