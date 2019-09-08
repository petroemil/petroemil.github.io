using BlazorSite.Markdown;
using Microsoft.AspNetCore.Components;

namespace BlazorSite.Shared
{
    public class MarkdownPresenterBase : ComponentBase
    {
        [Inject]
        private IMarkdownConverter? MarkdownConverter { get; set; }

        [Parameter]
        public string? PostId { get; set; }

        [Parameter]
        public string? Markdown { get; set; }

        protected string? FormattedMarkdown { get; set; }

        protected override void OnInitialized()
        {
            FormattedMarkdown = MarkdownConverter!.ConvertMarkdownToHTML(PostId!, Markdown!);
        }
    }
}