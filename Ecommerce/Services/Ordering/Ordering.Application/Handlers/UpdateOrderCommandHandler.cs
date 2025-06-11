using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Application.Exceptions;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Unit>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public UpdateOrderCommandHandler(IOrderRepository orderRepository, ILogger logger, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderToUpdate = await _orderRepository.GetByIdAsync(request.Id);
            if(orderToUpdate == null)
            {
                throw new OrderNotFoundException(nameof(Order), request.Id);
            }
            _mapper.Map(request, orderToUpdate, typeof(UpdateOrderCommand), typeof(Unit)); //Copie uniquement les propriétés mappées
            await _orderRepository.UpdateAsync(orderToUpdate);
            _logger.LogInformation($"Order {orderToUpdate.Id} is scuccessfully  updated");
            return Unit.Value;
        }
    }
}
