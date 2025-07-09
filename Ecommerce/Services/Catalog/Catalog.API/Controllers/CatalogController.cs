using Catalog.Application.Commands;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Specs;
using Common.Logging.Correlation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;


namespace Catalog.API.Controllers
{
    public class CatalogController : APIController
    {
        private readonly IMediator _mediator;

        public ILogger<CatalogController> _logger;
        private readonly ICorrelationIDGenerator _correlationIDGenerator;

        public CatalogController(IMediator mediator, ILogger<CatalogController> logger, ICorrelationIDGenerator correlationIDGenerator)
        {
            _mediator = mediator;
            _logger = logger;
            _correlationIDGenerator = correlationIDGenerator;
            _logger.LogInformation("Correlation Id {correlationId}", _correlationIDGenerator.Get());
        }

        [HttpGet]
        [Route("[action]/{id}", Name = "GetProductById")]
        [ProducesResponseType(typeof(ProductResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ProductResponse>> GetProductById(string id)
        {
            var query = new GetProductByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]/{productName}", Name = "GetProductByProductName")]
        [ProducesResponseType(typeof(IList<ProductResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IList<ProductResponse>>> GetProductByProductName(string productName)
        {
            var query = new GetProductByNameQuery(productName);
            var result = await _mediator.Send(query);
            _logger.LogInformation($" Produit avec le nom {productName} trouvé");
            return Ok(result);
        }

        [HttpGet]
        [Route("GetAllProducts")]
        [ProducesResponseType(typeof(Pagination<ProductResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IList<ProductResponse>>> GetAllProducts([FromQuery]CatalogSpecsParams catalogSpecsParams)
        {
            var query = new GetAllProductsQuery(catalogSpecsParams);
            var result = await _mediator.Send(query);
            _logger.LogInformation("Tous les produits sont récupérés.");
            return Ok(result);
        }


        [HttpGet]
        [Route("GetAllBrands")]
        [ProducesResponseType(typeof(IList<BrandResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BrandResponse>> GetAllBrands()
        {
            var query = new GetAllBrandsQuery();
            var result = await _mediator.Send(query);
            _logger.LogInformation("Tous les modèle de produits sont récupérés.");
            return Ok(result);
        }

        [HttpGet]
        [Route("GetAllTypes")]
        [ProducesResponseType(typeof(IList<TypesResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<TypesResponse>> GetAllTypes()
        {
            var query = new GetAllTypesQuery();
            var result = await _mediator.Send(query);
            _logger.LogInformation("Tous les types de produits sont récupérés.");
            return Ok(result);
        }


        [HttpGet]
        [Route("[action]/{brand}", Name = "GetProductByBrandName")]
        [ProducesResponseType(typeof(IList<ProductResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IList<ProductResponse>>> GetProductByBrandName(string brand)
        {
            var query = new GetProductByBrandQuery(brand);
            var result = await _mediator.Send(query);
            _logger.LogInformation($"Le produit avec le modèle {brand} est trouvé.");
            return Ok(result);
        }


        [HttpPost]
        [Route("CreateProduct")]
        [ProducesResponseType(typeof(ProductResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ProductResponse>> CreateProduct([FromBody] CreateProductCommand productCommand)
        {
            var result = await _mediator.Send<ProductResponse>(productCommand);
            _logger.LogInformation("Nouveau produit créé.");
            return Ok(result);
        }

        [HttpPut]
        [Route("UpdateProduct")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ProductResponse>> UpdateProduct([FromBody] UpdateProductCommand productCommand)
        {
            var result = await _mediator.Send(productCommand);
            _logger.LogInformation("Mise à jour du produit effectué.");
            return Ok(result);
        }

        [HttpDelete]
        [Route("{id}", Name = "DeleteProduct")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ProductResponse>> DeleteProduct(string id)
        {
            var command = new DeleteProductByIdCommand(id);
            var result = await _mediator.Send(command);
            _logger.LogInformation("Produit supprimé.");
            return Ok(result);
        }
    }
}
