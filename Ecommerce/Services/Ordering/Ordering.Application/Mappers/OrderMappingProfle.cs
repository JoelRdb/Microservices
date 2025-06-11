using AutoMapper;
using Ordering.Application.Responses;
using Ordering.Core.Entities;

namespace Ordering.Application.Mappers
{
    public class OrderMappingProfle : Profile
    {
        public OrderMappingProfle()
        {
            CreateMap<Order, OrderResponse>().ReverseMap();
        }
    }
}
