using System.Threading.Tasks;
using MyShop.ProductManagement.Api.Core;
using MyShop.ProductManagement.Api.Requests;

namespace MyShop.ProductManagement.Api.Services
{
    public interface IProductsService
    {
        Task<Result<int>> UpsertProductAsync(UpsertProductRequest request);
    }
}