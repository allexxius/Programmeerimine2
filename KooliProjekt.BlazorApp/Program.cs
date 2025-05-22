using KooliProjekt.BlazorApp;

using Microsoft.AspNetCore.Components.Web;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using System;

using System.Net.Http;

using System.Threading.Tasks;

namespace KooliProjekt.BlazorApp

{

    public class Program

    {

        public static async Task Main(string[] args)

        {

            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.RootComponents.Add<App>("#app");

            builder.RootComponents.Add<HeadOutlet>("head::after");

            // HttpClient, mis saadab p�ringuid sinu API aadressile

            builder.Services.AddScoped(sp => new HttpClient

            {

                BaseAddress = new Uri("https://localhost:7136/api/")

            });

            // ApiClient teenusena (kasutab HttpClientit, kui konstruktoris n�utakse)

            builder.Services.AddScoped<IApiClient, ApiClient>();

            await builder.Build().RunAsync();

        }

    }

}

