using System;

namespace BlazorSite.BlogService.Dto
{
    public class BlogPostMetadata
    {
        public bool IsDraft { get; set; }
        public DateTimeOffset PublishDate { get; set; }
        public string Title { get; set; }
        public string HeroImageFile { get; set; }
        public string SummaryMarkdownFile { get; set; }
        public string? MarkdownFile { get; set; }
        public string SocialSharingFile { get; set; }
    }
}