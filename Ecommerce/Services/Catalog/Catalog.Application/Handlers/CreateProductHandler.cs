using Catalog.Application.Commands;
using Catalog.Application.Mappers;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Handlers
{
    public class CreateProductHandler(IProductRepository repositoryProduct) : IRequestHandler<CreateProductCommand, ProductResponse>
    {
        private readonly IProductRepository _repositoryProduct;
        public async Task<ProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var productEntity = ProductMapper.Mapper.Map<Product>(request);
            if (productEntity is null)
            {
                throw new ApplicationException("Il y a un problème de mappage lors de la création d'un nouveau produit");
            }
            var newProduct = await _repositoryProduct.CreatProduct(productEntity);
            var productResponse = ProductMapper.Mapper.Map<ProductResponse>(newProduct);
            return productResponse;
        }
    }
}
