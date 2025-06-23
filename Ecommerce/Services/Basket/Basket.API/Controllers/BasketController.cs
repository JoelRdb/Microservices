using Basket.Application.Commands;
using Basket.Application.GrpcService;
using Basket.Application.Mappers;
using Basket.Application.Queries;
using Basket.Application.Responses;
using Basket.Core.Entities;
using EventBus.Messages.Common;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
    [Asp.Versioning.ApiVersion("1")]
    public class BasketController : APIController
    {

        public IMediator _mediator { get; }
        public ILogger<BasketController> _logger { get; }

        public IPublishEndpoint _publishEndpoint;

        public BasketController(IMediator mediator, IPublishEndpoint publishEndpoint, ILogger<BasketController> logger)
        {
            _mediator = mediator;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]/{userName}", Name="GetBasketByUserName")]
        [ProducesResponseType(typeof(ShoppingCartResponse), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCartResponse>> GetBasket(string userName)
        {
            var query = new GetBasketByUserNameQuery(userName);
            var basket = await _mediator.Send(query);
            return Ok(basket);
        }


        [HttpPost("CreateBasket")]
        [ProducesResponseType(typeof(ShoppingCartResponse), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCartResponse>> UpdateBasket([FromBody] CreateShoppingCartCommand createShoppingCartCommand)
        {            
            var basket = await _mediator.Send(createShoppingCartCommand);
            return Ok(basket);
        }

        [HttpDelete]
        [Route("[action]/{userName}", Name = "DeleteBasketByName")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        public async Task<ActionResult> DeleteBasket(string userName)
        {
            var cmd = new DeleteBasketByUserNameCommand(userName);
            var basket = await _mediator.Send(cmd);
            return Ok(basket);
        }


        //After developing EventBusMessages in Infrastructure

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            var query = new GetBasketByUserNameQuery(basketCheckout.UserName);
            var basket = await _mediator.Send(query);
            if(basket == null)
            {
                return BadRequest();
            }
            var eventMsg = BasketMapper.Mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMsg.TotalPrice = basket.TotalPrice;
            await _publishEndpoint.Publish(eventMsg);

            _logger.LogInformation($"Basket Published for {basket.UserName}");

            var deleteCmd = new DeleteBasketByUserNameCommand(basket.UserName);
            await _mediator.Send(deleteCmd);
            return Accepted();
        }
    }
}
