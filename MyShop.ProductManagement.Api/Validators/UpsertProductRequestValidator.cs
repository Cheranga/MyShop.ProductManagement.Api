using FluentValidation;
using MyShop.ProductManagement.Api.Extensions;
using MyShop.ProductManagement.Services.Requests;

namespace MyShop.ProductManagement.Api.Validators
{
    public class UpsertProductRequestValidator : ModelValidatorBase<UpsertProductRequest>
    {
        public UpsertProductRequestValidator()
        {
            RuleFor(x => x.ProductId).GreaterThanOrEqualTo(0);
            RuleFor(x => x.ProductCode).NotNullOrEmpty();
            RuleFor(x => x.ProductName).NotNullOrEmpty();
        }
    }
}
