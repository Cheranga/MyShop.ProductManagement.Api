using FluentValidation;
using FluentValidation.Results;
using Newtonsoft.Json;

namespace MyShop.ProductManagement.Api.Extensions
{
    public static class ValidationExtensions
    {
        public static string ToJson(this ValidationResult validationResult)
        {
            if (validationResult == null || validationResult.IsValid)
            {
                return string.Empty;
            }

            var serializedErrors = JsonConvert.SerializeObject(validationResult.Errors);
            return serializedErrors;
        }

        public static IRuleBuilderOptions<T, string> NotNullOrEmpty<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.NotNull().NotEmpty();
        }
    }
}
