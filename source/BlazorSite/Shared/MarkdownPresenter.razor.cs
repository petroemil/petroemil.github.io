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
        private HttpClient HttpClient { get; set; }

        [Inject]
        private IMarkdownConverter MarkdownConverter { get; set; }

        [Parameter]
        protected string Url { get; set; }

        protected string FormattedMarkdown { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            if (Url is null)
                return;

            var markdown = await HttpClient.GetStringAsync(Url);
            FormattedMarkdown = MarkdownConverter.ConvertMarkdownToHTML(markdown);

            this.StateHasChanged();
        }

        public void Dispose()
        {
            FormattedMarkdown = null;
        }
    }
}