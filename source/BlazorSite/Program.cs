using BlazorSite.BlogService;
using BlazorSite.Markdown;
using BlazorSite.ViewModels;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorSite
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args); 
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddSingleton<IMarkdownConverter, MarkdownConverter>();
            builder.Services.AddSingleton<ICodeHeighlighter, HighlightJs>();
            builder.Services.AddSingleton<IBlogService, BlogService.BlogService>();

            builder.Services.AddSingleton<IIndexViewModelProvider, IndexViewModelProvider>();
            builder.Services.AddSingleton<IPostViewModelProvider, PostViewModelProvider>();

            await builder.Build().RunAsync();
        }
    }
}
