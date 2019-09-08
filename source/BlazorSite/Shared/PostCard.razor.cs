using BlazorSite.BlogService;
using Microsoft.AspNetCore.Components;
using System;

namespace BlazorSite.Shared
{
    public class PostCardBase : ComponentBase
    {
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

        //protected string GetLinkedInShareUrl()
        //{
        //    var url = Uri.EscapeDataString(BlogPostUriHelper.GetSocialShareUrl(PostDetails.PostId, "socialshare.html"));

        //    return $"http://www.linkedin.com/shareArticle?mini=true&url={url}";            
        //}

        protected string GetSocialShareUrl()
            => BlogPostUriHelper.GetSocialShareUrl(PostDetails!.PostId, PostDetails.SocialSharingFile);
    }
}
