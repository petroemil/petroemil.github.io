using BlazorSite.BlogService;
using BlazorSite.Markdown;
using BlazorSite.ViewModels;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorSite
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IMarkdownConverter, MarkdownConverter>();
            services.AddSingleton<ICodeHeighlighter, HighlightJs>();
            services.AddSingleton<IBlogService, BlogService.BlogService>();

            services.AddSingleton<IIndexViewModelProvider, IndexViewModelProvider>();
            services.AddSingleton<IPostViewModelProvider, PostViewModelProvider>();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}