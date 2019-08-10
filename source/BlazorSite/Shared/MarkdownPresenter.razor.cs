using BlazorSite.Markdown;
using Microsoft.AspNetCore.Components;

namespace BlazorSite.Shared
{
    public class MarkdownPresenterBase : ComponentBase
    {
        [Inject]
        private IMarkdownConverter? MarkdownConverter { get; set; }

        [Parameter]
        private string? PostId { get; set; }

        [Parameter]
        private string? Markdown { get; set; }

        protected string? FormattedMarkdown { get; set; }

        protected override void OnInit()
        {
            FormattedMarkdown = MarkdownConverter!.ConvertMarkdownToHTML(PostId!, Markdown!);
        }
    }
}