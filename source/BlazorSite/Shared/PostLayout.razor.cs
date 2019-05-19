using BlazorSite.BlogService;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Layouts;

namespace BlazorSite.Shared
{
    public class PostLayoutBase : LayoutComponentBase
    {
        [Parameter]
        protected RenderFragment ChildContent { get; set; }

        [Parameter]
        protected BlogPostDetails PostDetails { get; set; }
    }
}
