using MediatR;
using MyShop.ProductManagement.Api.Core;

namespace MyShop.ProductManagement.Api.DataAccess
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