using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyShop.ProductManagement.Api.Requests;
using MyShop.ProductManagement.Api.Responses;

namespace MyShop.ProductManagement.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{v:apiVersion}/[controller]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class ProductsController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> PostProduct([FromBody]CreateProductRequest createProductRequest)
        {
            await Task.Delay(TimeSpan.FromSeconds(2)).ConfigureAwait(false);

            return CreatedAtAction(nameof(GetProductById), new { productId = createProductRequest.ProductId }, createProductRequest.ProductId);
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
