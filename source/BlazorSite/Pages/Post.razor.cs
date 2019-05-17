using System.Threading.Tasks;
using BlazorSite.BlogService;
using Microsoft.AspNetCore.Components;

namespace BlazorSite.Pages
{
    public class PostBase : ComponentBase
    {
        [Inject]
        private IBlogService BlogService { get; set; }

        [Parameter]
        private string PostId { get; set; }

        protected BlogPostMetadata Metadata { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            if (PostId is null)
                return;

            Metadata = await BlogService.GetBlogPost(PostId);

            this.StateHasChanged();
        }
    }
}