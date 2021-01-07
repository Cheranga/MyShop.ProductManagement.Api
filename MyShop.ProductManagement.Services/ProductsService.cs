using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using MyShop.ProductManagement.Core;
using MyShop.ProductManagement.DataAccess.CommandHandlers;
using MyShop.ProductManagement.DataAccess.Models;
using MyShop.ProductManagement.DataAccess.Queries;
using MyShop.ProductManagement.Services.Requests;

namespace MyShop.ProductManagement.Api.Services
{
    public class ProductsService : IProductsService
    {
        private readonly ILogger<ProductsService> _logger;
        private readonly IMediator _mediator;

        public ProductsService(IMediator mediator, ILogger<ProductsService> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Result<string>> UpsertProductAsync(UpsertProductRequest request)
        {
            _logger.LogInformation("Upserting product {correlationId}", request.CorrelationId);

            var operation = await _mediator.Send(new UpsertProductCommand(request.ProductId, request.ProductCode, request.ProductName));
            if (!operation.Status)
            {
                _logger.LogError("{correlationId} Error occured when upserting product. {upsertProductRequest}", request.CorrelationId, request);
                return Result<string>.Failure(operation.Validation);
            }

            _logger.LogInformation("{correlationId} Upserting product successful. {upsertProductRequest}", request.CorrelationId, request);

            return Result<string>.Success(operation.Data);
        }

        public async Task<Result<ProductDataModel>> GetProductAsync(GetProductRequest request)
        {
            _logger.LogInformation("Getting product {correlationId}", request.CorrelationId);

            var operation = await _mediator.Send(new GetProductQuery(request.ProductCode));
            if (!operation.Status)
            {
                _logger.LogError("{correlationId} error occured when getting the product.", request.CorrelationId);
                return Result<ProductDataModel>.Failure("", "Error occured when getting the product.");
            }

            var product = operation.Data;

            if (product == null)
            {
                return Result<ProductDataModel>.Failure("", "Product not found.");
            }

            return Result<ProductDataModel>.Success(product);
        }
    }
}