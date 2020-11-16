using BlazorSite.BlogService;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;

namespace BlazorSite.Shared
{
    public class PostCardBase : ComponentBase
    {
        [Inject]
        public IJSRuntime JSInterop { get; set; }

        [Parameter]
        public BlogPostDetails? PostDetails { get; set; }

        [Parameter]
        public bool? IsDetailedView { get; set; }

        protected string GetTwitterShareUrl()
        {
            var title = Uri.EscapeDataString(PostDetails!.Title);
            var url = Uri.EscapeDataString(GetSocialShareUrl());

            return $"https://twitter.com/intent/tweet?text={title}&url={url}";
        }

        protected string GetFacebookShareUrl()
        {
            var title = Uri.EscapeDataString(PostDetails!.Title);
            var url = Uri.EscapeDataString(GetSocialShareUrl());

            return $"https://www.facebook.com/sharer/sharer.php?text={title}&u={url}";
        }

        protected void OnShareButtonClick()
        {
            this.JSInterop.InvokeVoidAsync("navigator.share", new
            {
                title = this.PostDetails!.Title,
                url = this.GetSocialShareUrl()
            });
        }

        protected string GetSocialShareUrl()
            => BlogPostUriHelper.GetSocialShareUrl(PostDetails!.PostId, PostDetails.SocialSharingFile);
    }
}
