using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MyShop.ProductManagement.Api.Configs;
using MyShop.ProductManagement.Api.Core;

namespace MyShop.ProductManagement.Api.DataAccess
{
    public class UpsertProductCommandHandler : IRequestHandler<UpsertProductCommand, Result<int>>
    {
        private const string InsertCommand = "insert into Products (ProductCode, ProductName) " +
                                             "output inserted.Id, inserted.ProductCode, inserted.ProductName " +
                                             "values (@ProductCode, @ProductName)";


        private const string UpdateCommand = "update Products set ProductCode=@ProductCode, ProductName=@ProductName " +
                                             "output inserted.Id, inserted.ProductCode, inserted.ProductName " +
                                             "where id=@Id";

        private readonly string _connectionString;


        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<UpsertProductCommandHandler> _logger;


        public UpsertProductCommandHandler(DatabaseConfig databaseConfig, IDbConnectionFactory dbConnectionFactory, ILogger<UpsertProductCommandHandler> logger)
        {
            _connectionString = databaseConfig.ConnectionString;
            _dbConnectionFactory = dbConnectionFactory;
            _logger = logger;
        }

        public async Task<Result<int>> Handle(UpsertProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var command = request.Id <= 0 ? InsertCommand : UpdateCommand;

                using (var connection = _dbConnectionFactory.GetConnection(_connectionString))
                {
                    var upsertedProducts = await connection.QueryAsync<ProductDataModel>(command, request);
                    var upsertedProduct = upsertedProducts.FirstOrDefault();
                    if (upsertedProduct == null)
                    {
                        _logger.LogError("Error when upserting product {command}", request);
                        return Result<int>.Failure("", "Error occured when upserting product.");
                    }

                    return Result<int>.Success(upsertedProduct.Id);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error occured when upserting product {command}", request);
            }

            return Result<int>.Failure("", "Error occured when upserting product.");
        }
    }
}