using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Handlers
{
    public class GetProductByBrandHandler(IProductRepository repositoryProduct) : IRequestHandler<GetProductByBrandQuery, IList<ProductResponse>>
    {
        private readonly IProductRepository _repositoryProduct = repositoryProduct;
        public async Task<IList<ProductResponse>> Handle(GetProductByBrandQuery request, CancellationToken cancellationToken)
        {
            var products = await _repositoryProduct.GetProductsByBrand(request.BrandName);
            var productsList = ProductMapper.Mapper.Map<IList<ProductResponse>>(products);
            return productsList;
        }
    }
}
