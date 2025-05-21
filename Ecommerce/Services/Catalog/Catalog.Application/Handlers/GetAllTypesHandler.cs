
using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class GetAllTypesHandler (ITypeReposiroty reposiroty): IRequestHandler<GetAllTypesQuery, IList<TypesResponse>>
    {
        private readonly ITypeReposiroty _repository = reposiroty;

        public async Task<IList<TypesResponse>> Handle(GetAllTypesQuery request, CancellationToken cancellationToken)
        {
            var types = await _repository.GetAllTypes();
            var typesResponseList = ProductMapper.Mapper.Map<IList<TypesResponse>>(types);
            return typesResponseList;
        }
    }
}
