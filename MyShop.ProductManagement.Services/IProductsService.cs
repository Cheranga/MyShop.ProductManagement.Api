using System.Threading.Tasks;
using MyShop.ProductManagement.Core;
using MyShop.ProductManagement.DataAccess.Models;
using MyShop.ProductManagement.Services.Requests;

namespace MyShop.ProductManagement.Api.Services
{
    public interface IProductsService
    {
        Task<Result<int>> UpsertProductAsync(UpsertProductRequest request);
        Task<Result<ProductDataModel>> GetProductAsync(GetProductRequest request);
    }
}