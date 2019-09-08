using Microsoft.AspNetCore.Components;

namespace BlazorSite.Shared
{
    public class ConditionalLinkBase : LayoutComponentBase
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        [Parameter]
        public string? Uri { get; set; }
    }
}
