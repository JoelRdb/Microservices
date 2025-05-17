using AutoMapper;
using Catalog.Application.Responses;
using Catalog.Core.Entities;

namespace Catalog.Application.Mappers
{
    public class ProductMappingProfile : Profile //Par AutoMapper
    {
        public ProductMappingProfile()
        {
            CreateMap<ProductBrand, BrandResponse>().ReverseMap(); //CreateMap<Source, Destination>  ; ReverseMap : rend le mappage bidirectionnel
        }
    }
}
