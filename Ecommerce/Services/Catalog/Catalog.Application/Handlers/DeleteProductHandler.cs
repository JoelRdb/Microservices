using Catalog.Application.Commands;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class DeleteProductHandler(IProductRepository repository) : IRequestHandler<DeleteProductByIdCommand, bool>
    {
        private readonly IProductRepository _repository = repository;
      
        public async Task<bool> Handle(DeleteProductByIdCommand request, CancellationToken cancellationToken)
        {
           return await _repository.DeleteProduct(request.Id);
            
        }
    }
}
