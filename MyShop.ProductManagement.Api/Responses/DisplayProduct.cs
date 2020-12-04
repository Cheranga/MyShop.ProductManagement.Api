using System.ComponentModel.DataAnnotations;

namespace MyShop.ProductManagement.Api.Responses
{
    public class DisplayProduct
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}