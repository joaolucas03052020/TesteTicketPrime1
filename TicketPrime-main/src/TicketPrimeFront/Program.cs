using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TicketPrimeFront;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// URL base da API — ajuste a porta conforme o launchSettings.json da API
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5246") });

await builder.Build().RunAsync();
