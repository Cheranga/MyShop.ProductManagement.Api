using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyShop.ProductManagement.Api.Requests;
using MyShop.ProductManagement.Api.Responses;
using MyShop.ProductManagement.Api.Services;

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

        public ProductsController(IProductsService productService, IValidator<UpsertProductRequest> upsertProductRequestValidator)
        {
            _productService = productService;
            _upsertProductRequestValidator = upsertProductRequestValidator;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> PostProduct([FromBody]UpsertProductRequest createProductRequest)
        {
            var validationResult = await _upsertProductRequestValidator.ValidateAsync(createProductRequest);
            if (!validationResult.IsValid)
            {
                //
                // TODO: Do the validation result transformations.
                //
                return BadRequest(validationResult);
            }

            var operation = await _productService.UpsertProductAsync(createProductRequest);

            if (operation.Status)
            {
                return CreatedAtAction(nameof(GetProductById), new { productId = operation.Data }, null);
            }

            return StatusCode((int) (HttpStatusCode.InternalServerError));

        }

        [HttpGet("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(DisplayProduct))]
        public async Task<IActionResult> GetProductById(string productId)
        {
            await Task.Delay(TimeSpan.FromSeconds(2)).ConfigureAwait(false);

            var dto = new DisplayProduct
            {
                Id = productId,
                Name = "TODO: Get the product name from the storage"
            };

            return Ok(dto);
        }
    }
}
