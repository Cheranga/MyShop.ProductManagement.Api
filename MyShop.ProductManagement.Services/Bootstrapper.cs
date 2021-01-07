using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyShop.ProductManagement.Api.Services;
using MyShop.ProductManagement.DataAccess;

namespace MyShop.ProductManagement.Services
{
    public static class Bootstrapper
    {
        public static void UseProductsServices(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
            {
                return;
            }

            services.AddScoped<IProductsService, ProductsService>();

            services.UseProductsDataAccess(configuration);

        }
    }
}