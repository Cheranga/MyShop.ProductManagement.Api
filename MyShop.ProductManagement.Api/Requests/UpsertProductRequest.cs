using System;

namespace MyShop.ProductManagement.Api.Requests
{
    public class UpsertProductRequest
    {
        public string CorrelationId { get; set; } = Guid.NewGuid().ToString("N");
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
    }
}