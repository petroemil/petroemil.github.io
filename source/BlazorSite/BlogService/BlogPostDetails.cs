using BlazorSite.BlogService.Dto;
using System;

namespace BlazorSite.BlogService
{
    public class BlogPostDetails
    {
        public string PostId { get; }
        public bool IsDraft { get; }
        public DateTimeOffset PublishDate { get; }
        public string Title { get; }
        public string HeroImageFile { get; }
        public string SummaryMarkdownFile {get; }
        public string? MarkdownFile { get; }

        public BlogPostDetails(string postId, BlogPostMetadata metadata)
        {
            PostId = postId;

            IsDraft = metadata.IsDraft;
            PublishDate = metadata.PublishDate;
            Title = metadata.Title;
            HeroImageFile = metadata.HeroImageFile;
            SummaryMarkdownFile = metadata.SummaryMarkdownFile;
            MarkdownFile = metadata.MarkdownFile;
        }
    }
}