using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Layouts;

namespace BlazorSite.Shared
{
    public class ConditionalLinkBase : LayoutComponentBase
    {
        [Parameter]
        protected RenderFragment? ChildContent { get; set; }

        [Parameter]
        protected string? Uri { get; set; }
    }
}
