using MediatR;
using MyShop.ProductManagement.Api.Core;

namespace MyShop.ProductManagement.Api.DataAccess
{
    public class UpsertProductCommand : IRequest<Result<int>>
    {
        public UpsertProductCommand(int id, string productCode, string productName)
        {
            Id = id;
            ProductCode = productCode;
            ProductName = productName;
        }

        public int Id { get; }
        public string ProductCode { get; }
        public string ProductName { get; }
    }
}