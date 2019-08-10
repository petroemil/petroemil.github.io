using BlazorSite.BlogService;
using BlazorSite.Markdown;
using Microsoft.AspNetCore.Components;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorSite.Shared
{
    public class MarkdownPresenterBase : ComponentBase, IDisposable
    {
        [Inject]
        private HttpClient? HttpClient { get; set; }

        [Inject]
        private IMarkdownConverter? MarkdownConverter { get; set; }

        [Parameter]
        private string? PostId { get; set; }

        [Parameter]
        private string? FileName { get; set; }

        protected string? FormattedMarkdown { get; set; }

        protected override async Task OnInitAsync()
        {
            if (PostId is null || FileName is null)
                return;

            var markdownFilePath = BlogPostUriHelper.GetContentFileUri(PostId, FileName);
            var markdown = await HttpClient!.GetStringAsync(markdownFilePath);

            FormattedMarkdown = MarkdownConverter!.ConvertMarkdownToHTML(PostId, markdown);
        }

        public void Dispose()
        {
            FormattedMarkdown = null;
        }
    }
}