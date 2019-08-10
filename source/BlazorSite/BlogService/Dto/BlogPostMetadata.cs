using System;

namespace BlazorSite.BlogService.Dto
{
    public class BlogPostMetadata
    {
#pragma warning disable CS8618 // Non-nullable field is uninitialized.
        public bool IsDraft { get; set; }
        public DateTimeOffset PublishDate { get; set; }
        public string Title { get; set; }
        public string HeroImageFile { get; set; }
        public string SummaryMarkdownFile { get; set; }
        public string SocialSharingFile { get; set; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized.
        public string? MarkdownFile { get; set; }
    }
}