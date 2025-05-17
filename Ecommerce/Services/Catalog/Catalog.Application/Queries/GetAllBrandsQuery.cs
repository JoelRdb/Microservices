
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using MediatR;

namespace Catalog.Application.Queries
{
    public class GetAllBrandsQuery : IRequest<IList<BrandResponse>>
    {
    }
}
