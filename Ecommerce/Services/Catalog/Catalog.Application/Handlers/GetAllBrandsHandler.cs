
using AutoMapper;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class GetAllBrandsHandler(IBrandRepository repository, IMapper mapper) : IRequestHandler<GetAllBrandsQuery, IList<BrandResponse>>
    {

        private readonly IBrandRepository _repository = repository;

        private readonly IMapper _mapper = mapper;

        public async Task<IList<BrandResponse>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
        {
            var brandList = await _repository.GetAllBrands();
            var brandResponseList = _mapper.Map<IList<ProductBrand>, IList<BrandResponse>>(brandList.ToList());
            return brandResponseList;
        }
    }
}
