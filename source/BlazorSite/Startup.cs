using BlazorSite.BlogService;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorSite
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ICodeHeighlighter, HighlightJs>();
            services.AddSingleton<IBlogService, BlogService.BlogService>();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");

            CustomCodeFormatter.Configure(app.Services);
        }
    }
}