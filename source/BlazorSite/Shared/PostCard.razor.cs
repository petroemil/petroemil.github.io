using BlazorSite.BlogService;
using Microsoft.AspNetCore.Components;

namespace BlazorSite.Shared
{
    public class PostCardBase : ComponentBase
    {
        [Parameter]
        protected BlogPostDetails PostDetails { get; set; }

        [Parameter]
        protected bool IsDetailedView { get; set; }
    }
}
