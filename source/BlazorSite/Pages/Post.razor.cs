using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace BlazorSite.Pages
{
    public class PostMetadata
    {
        public DateTimeOffset PublishDate { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
    }

    public class PostBase : ComponentBase
    {
        [Inject]
        private HttpClient HttpClient { get; set; }

        [Parameter]
        protected string PostId { get; set; }

        protected string HeroImage => $"content/{PostId}/heroimage.jpg";
        protected string Content => $"content/{PostId}/post.md";

        protected PostMetadata Metadata { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            if (PostId is null)
                return;

            Metadata = await HttpClient.GetJsonAsync<PostMetadata>($"content/{PostId}/metadata.json");

            Console.WriteLine(Metadata is null);

            this.StateHasChanged();
        }
    }
}