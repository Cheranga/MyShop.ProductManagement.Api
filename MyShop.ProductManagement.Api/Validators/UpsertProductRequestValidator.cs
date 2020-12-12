using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using MyShop.ProductManagement.Api.Requests;

namespace MyShop.ProductManagement.Api.Validators
{
    public class UpsertProductRequestValidator : ModelValidatorBase<UpsertProductRequest>
    {
        public UpsertProductRequestValidator()
        {
            RuleFor(x => x.ProductId).GreaterThanOrEqualTo(0);
            RuleFor(x => x.ProductName).NotNull().NotEmpty();
            RuleFor(x => x.ProductName).NotNull().NotEmpty();
        }
    }
}
