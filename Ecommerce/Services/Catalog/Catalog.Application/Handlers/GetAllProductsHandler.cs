using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using DnsClient.Internal;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Handlers
{
    public class GetAllProductsHandler(IProductRepository productRepository, ILogger<GetAllProductsHandler> logger) : IRequestHandler<GetAllProductsQuery, Pagination<ProductResponse>>
    {
        private readonly IProductRepository _productRepository = productRepository;
        private readonly ILogger<GetAllProductsHandler> _logger = logger;

        public async Task<Pagination<ProductResponse>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var productList = await _productRepository.GetProducts(request.CatalogSpecsParams);
            var productResponseList = ProductMapper.Mapper.Map<Pagination<ProductResponse>>(productList);
            _logger.LogDebug("Handler : Liste de tous les produits reçu {@productList}, Nombre de produits :", productList, productResponseList.Count);
            return productResponseList;
        }
    }
}
