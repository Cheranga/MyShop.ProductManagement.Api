using MediatR;
using MyShop.ProductManagement.Core;
using MyShop.ProductManagement.DataAccess.Models;

namespace MyShop.ProductManagement.DataAccess.Queries
{
    public class GetProductQuery : IRequest<Result<ProductDataModel>>
    {
        public string ProductCode { get; }

        public GetProductQuery(string productCode)
        {
            ProductCode = productCode;
        }
    }
}