﻿using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using MyShop.ProductManagement.Api.Core;
using MyShop.ProductManagement.Api.DataAccess;
using MyShop.ProductManagement.Api.Requests;

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

        public async Task<Result<int>> UpsertProductAsync(UpsertProductRequest request)
        {
            _logger.LogInformation("Upserting product {correlationId}", request.CorrelationId);

            var operation = await _mediator.Send(new UpsertProductCommand(request.ProductId, request.ProductCode, request.ProductName));
            if (!operation.Status)
            {
                _logger.LogError("{correlationId} Error occured when upserting product. {upsertProductRequest}", request.CorrelationId, request);
                return Result<int>.Failure(operation.Validation);
            }

            _logger.LogInformation("{correlationId} Upserting product successful. {upsertProductRequest}", request.CorrelationId, request);

            return Result<int>.Success(operation.Data);
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