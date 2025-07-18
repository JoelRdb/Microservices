﻿using Basket.Application.Commands;
using Basket.Application.GrpcService;
using Basket.Application.Mappers;
using Basket.Application.Queries;
using Basket.Application.Responses;
using Basket.Core.Entities;
using Common.Logging.Correlation;
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
        public ICorrelationIDGenerator _correlationIDGenerator { get; }

        public IPublishEndpoint _publishEndpoint;

        public BasketController(IMediator mediator, IPublishEndpoint publishEndpoint, ILogger<BasketController> logger, ICorrelationIDGenerator correlationIDGenerator)
        {
            _mediator = mediator;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
            _correlationIDGenerator = correlationIDGenerator;
            _logger.LogInformation("Correlation Id {correlationId}", _correlationIDGenerator.Get());
        }

        /// <summary>Je suis une méthode testée pour Swagger doc</summary>
        /// <param name="userName">Nom de l'utilisateur</param>
        /// <returns>Le panier</returns>
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
            _logger.LogInformation("Panier créé");
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


        // After developing EventBusMessages in Infrastructure : 
        // Update appsettings.json for EventBusSettings,
        // Update dockerfile (COPY ["Infrastructure/EventBus.Messages/EventBus.Messages.csproj", "Infrastucture/EventBus.Messages/"]), 
        // Update Program.cs for MassTransit(RabbitMQ).
        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            // Get existing basket with username
            var query = new GetBasketByUserNameQuery(basketCheckout.UserName);
            var basket = await _mediator.Send(query);
            if(basket == null)
            {
                return BadRequest();
            }
            var eventMsg = BasketMapper.Mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMsg.TotalPrice = basket.TotalPrice;
            eventMsg.CorrelationId = _correlationIDGenerator.Get();
            await _publishEndpoint.Publish(eventMsg);

            _logger.LogInformation($"Basket Published for {basket.UserName}");
            //remove the basket
            var deleteCmd = new DeleteBasketByUserNameCommand(basket.UserName);
            await _mediator.Send(deleteCmd);
            return Accepted();
        }
    }
}
