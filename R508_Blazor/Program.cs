using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using R508_Blazor.Models;
using R508_Blazor.Services.Implementation;
using R508_Blazor.Services.Interface;
using R508_Blazor.ViewModels;

namespace R508_Blazor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7054/")
            });

            builder.Services.AddScoped<ProductWebService>();
            builder.Services.AddScoped<BrandWebService>();
            builder.Services.AddScoped<ProductTypeWebService>();

            builder.Services.AddScoped<IService<ProductDto, ProductCreateDto>>(sp => sp.GetRequiredService<ProductWebService>());
            builder.Services.AddScoped<IService<BrandDto, BrandCreateDto>>(sp => sp.GetRequiredService<BrandWebService>());
            builder.Services.AddScoped<IService<ProductTypeDto, ProductTypeCreateDto>>(sp => sp.GetRequiredService<ProductTypeWebService>());

            builder.Services.AddScoped<ProductsViewModel>();
            builder.Services.AddScoped<BrandsViewModel>();
            builder.Services.AddScoped<ProductTypesViewModel>();

            await builder.Build().RunAsync();
        }
    }
}