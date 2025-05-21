
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using MediatR;

namespace Catalog.Application.Queries
{
    public class GetAllTypesQuery : IRequest<IList<TypesResponse>>
    {
    }
}
