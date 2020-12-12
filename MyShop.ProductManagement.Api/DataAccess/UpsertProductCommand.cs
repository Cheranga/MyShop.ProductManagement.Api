using MediatR;
using MyShop.ProductManagement.Api.Core;

namespace MyShop.ProductManagement.Api.DataAccess
{
    public class UpsertProductCommand : IRequest<Result>
    {
        public UpsertProductCommand(int id, string productCode, string productDescription)
        {
            Id = id;
            ProductCode = productCode;
            ProductDescription = productDescription;
        }

        public int Id { get; }
        public string ProductCode { get; }
        public string ProductDescription { get; }
    }
}