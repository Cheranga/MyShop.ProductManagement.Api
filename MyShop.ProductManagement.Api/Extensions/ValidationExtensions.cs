using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}
