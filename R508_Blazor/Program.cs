using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using R508_Blazor.Models;
using R508_Blazor.Services;
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
            builder.Services.AddScoped<IService<ProduitDto, ProduitCreateDto>, WSService>();
            builder.Services.AddScoped<ProductsViewModel>();

            builder.Services.AddScoped<ProductsViewModel>();

            await builder.Build().RunAsync();
        }
    }
}