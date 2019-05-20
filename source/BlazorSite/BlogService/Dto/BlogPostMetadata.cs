using System;

namespace BlazorSite.BlogService.Dto
{
    public class BlogPostMetadata
    {
        public bool IsDraft { get; private set; }
        public DateTimeOffset PublishDate { get; private set; }
        public string Title { get; private set; }
        public string HeroImageFile { get; private set; }
        public string SummaryMarkdownFile { get; private set; }
        public string? MarkdownFile { get; private set; }
    }
}