using System.Text.Json.Serialization;

namespace MyShop.ProductManagement.Services.Requests
{
    public class GetProductRequest
    {
        [JsonIgnore]
        public string CorrelationId { get; set; }

        public string ProductCode { get; set; }
    }
}