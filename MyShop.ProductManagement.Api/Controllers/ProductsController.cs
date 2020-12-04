using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyShop.ProductManagement.Api.Requests;

namespace MyShop.ProductManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostProduct(CreateProductRequest createProductRequest)
        {
            await Task.Delay(TimeSpan.FromSeconds(2)).ConfigureAwait(false);

            return CreatedAtAction(nameof(GetProductById), new { productId = createProductRequest.ProductId }, createProductRequest.ProductId);
        }

        [HttpGet("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProductById(string productId)
        {
            await Task.Delay(TimeSpan.FromSeconds(2)).ConfigureAwait(false);

            return Ok(new
            {
                id = productId
            });
        }
    }
}
