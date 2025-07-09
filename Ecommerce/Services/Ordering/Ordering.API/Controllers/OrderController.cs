using Common.Logging.Correlation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Commands;
using Ordering.Application.Queries;
using Ordering.Application.Responses;
using Ordering.Infrastructure.Repositories;
using System.Net;

namespace Ordering.API.Controllers
{
    public class OrderController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<OrderController> _logger;
        private readonly ICorrelationIDGenerator _correlationIDGenerator;

        public OrderController(IMediator mediator, ILogger<OrderController> logger, ICorrelationIDGenerator correlationIDGenerator)
        {
            _mediator = mediator;
            _logger = logger;
            _correlationIDGenerator = correlationIDGenerator;
            _logger.LogInformation("Correlation Id {correlationId}", _correlationIDGenerator.Get());
        }

        [HttpGet("{userName}", Name = "GetOrderByUserName")]
        [ProducesResponseType(typeof(IEnumerable<OrderResponse>), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrdersByUserName(string userName)
        {
            var query = new GetOrderListQuery(userName);
            var orders = await _mediator.Send(query);
            return Ok(orders);
        }

        // Just for testing
        [HttpPost(Name ="CheckoutOrder")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<int>> CheckoutOrder([FromBody] CheckoutOrderCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut(Name = "UpdateOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> UpdateOrder([FromBody] UpdateOrderCommand command)
        {
            var resut = await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("[action]/{id}", Name = "DeleteOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            var cmd = new DeleteOrderCommand()
            {
                Id = id
            };
            await _mediator.Send(cmd);
            return NoContent();
        }
    }
}
