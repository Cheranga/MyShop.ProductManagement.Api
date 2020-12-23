namespace MyShop.ProductManagement.Api.Requests
{
    public class UpsertProductRequest
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
    }
}