
using Catalog.Application.Responses;
using Catalog.Core.Specs;
using MediatR;

namespace Catalog.Application.Queries
{
    public class GetAllProductsQuery : IRequest<Pagination<ProductResponse>>
    {
        public CatalogSpecsParams CatalogSpecsParams { get; set; }

        public GetAllProductsQuery(CatalogSpecsParams catalogSpecsParams)
        {
            CatalogSpecsParams = catalogSpecsParams;
        }
    }
}
