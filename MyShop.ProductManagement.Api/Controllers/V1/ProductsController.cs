using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyShop.ProductManagement.Api.Extensions;
using MyShop.ProductManagement.Api.Responses;
using MyShop.ProductManagement.Api.Services;
using MyShop.ProductManagement.Services.Requests;

namespace MyShop.ProductManagement.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{v:apiVersion}/[controller]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productService;
        private readonly IValidator<UpsertProductRequest> _upsertProductRequestValidator;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductsService productService, IValidator<UpsertProductRequest> upsertProductRequestValidator,
            ILogger<ProductsController> logger)
        {
            _productService = productService;
            _upsertProductRequestValidator = upsertProductRequestValidator;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> PostProduct(UpsertProductRequest createProductRequest, [FromHeader(Name = "correlationId")]string correlationId)
        {
            var validationResult = await _upsertProductRequestValidator.ValidateAsync(createProductRequest);
            if (!validationResult.IsValid)
            {
                //
                // TODO: Do the validation result transformations.
                //
                _logger.LogWarning("{correlationId} invalid request received.", createProductRequest?.CorrelationId);
                return BadRequest(validationResult);
            }

            correlationId = string.IsNullOrWhiteSpace(correlationId) ? Guid.NewGuid().ToString("N") : correlationId;
            createProductRequest.CorrelationId = correlationId;

            _logger.LogInformation("{correlationId} starting to upsert product.", createProductRequest.CorrelationId);
            var operation = await _productService.UpsertProductAsync(createProductRequest);

            if (operation.Status)
            {
                _logger.LogInformation("{correlationId} successfully upserted product.", createProductRequest.CorrelationId);
                return CreatedAtAction(nameof(GetProductById), new { productCode = operation.Data }, null);
            }

            _logger.LogError("{correlationId} {errorReason} error when upserting product.", createProductRequest.CorrelationId, operation.Validation.ToJson());

            return StatusCode((int)(HttpStatusCode.InternalServerError));

        }

        [HttpGet("{productCode}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DisplayProduct))]
        public async Task<IActionResult> GetProductById([FromRoute]string productCode, [FromHeader(Name = "correlationId")] string correlationId)
        {
            correlationId = string.IsNullOrWhiteSpace(correlationId) ? Guid.NewGuid().ToString("N") : correlationId;
            var getProductRequest = new GetProductRequest
            {
                CorrelationId = correlationId,
                ProductCode = productCode
            };

            var operation = await _productService.GetProductAsync(getProductRequest);
            if (operation.Status)
            {
                _logger.LogInformation("{correlationId} successfully returned product", correlationId);
                return Ok(operation.Data);
            }

            _logger.LogWarning("{correlationId} {message}",correlationId, operation.Validation.ToJson());
            return NotFound();
        }
    }
}
