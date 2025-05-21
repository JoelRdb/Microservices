using Catalog.Application.Commands;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;


namespace Catalog.Application.Handlers
{
    public class UpdateProductHandler(IProductRepository repository) : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IProductRepository _repository = repository;
        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var productEntity = await _repository.UpdateProduct(new Product
            {
                Id = request.Id,
                Name = request.Name,
                Summary = request.Summary,
                Description = request.Description,
                Brands = request.Brands,
                Types = request.Types,
                ImageFile = request.ImageFile,
                Price = request.Price
            });
            return true;
        }
    }
}
