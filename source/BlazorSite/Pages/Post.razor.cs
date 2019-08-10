using System.Threading.Tasks;
using BlazorSite.BlogService;
using Microsoft.AspNetCore.Components;

namespace BlazorSite.Pages
{
    public class PostBase : ComponentBase
    {
        [Inject]
        private IBlogService? BlogService { get; set; }

        [Parameter]
        private string? PostId { get; set; }

        protected BlogPostDetails? PostDetails { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            if (PostId is null)
                return;

            PostDetails = await BlogService!.GetBlogPostDetails(PostId);

            this.StateHasChanged();
        }
    }
}