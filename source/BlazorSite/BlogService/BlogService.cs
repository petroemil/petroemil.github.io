using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorSite.BlogService
{
    public class BlogPostMetadata
    {
        public string PostId { get; set; }
        public DateTimeOffset PublishDate { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string HeroImageUrl { get; set; }
        public string MarkdownContentUrl { get; set; }
    }

    public interface IBlogService
    {
        Task<IEnumerable<BlogPostMetadata>> GetBlogPosts();
        Task<BlogPostMetadata> GetBlogPost(string postId);
    }

    public class BlogService : IBlogService
    {
        private readonly HttpClient httpClient;

        public BlogService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public Task<BlogPostMetadata> GetBlogPost(string postId)
        {
            return httpClient.GetJsonAsync<BlogPostMetadata>($"content/{postId}/metadata.json");
        }

        public async Task<IEnumerable<BlogPostMetadata>> GetBlogPosts()
        {
            return new[]
            {
                new BlogPostMetadata
                {
                    PostId = "test-post",
                    Title = "Hello World #1",
                    Summary = "This is a fake summary for this post, not something coming from the JSON metadata",
                    PublishDate = DateTimeOffset.Now,
                    HeroImageUrl = "heroimage.jpg",
                    MarkdownContentUrl = "post.md"
                },
                new BlogPostMetadata
                {
                    PostId = "test-post",
                    Title = "Hello World #2",
                    Summary = "Still not the real thing",
                    PublishDate = DateTimeOffset.Now - TimeSpan.FromHours(1),
                    HeroImageUrl = "heroimage.jpg",
                    MarkdownContentUrl = "post.md"
                },
                new BlogPostMetadata
                {
                    PostId = "test-post",
                    Title = "Hello World #3",
                    Summary = "Still not...",
                    PublishDate = DateTimeOffset.Now - TimeSpan.FromHours(2),
                    HeroImageUrl = "heroimage.jpg",
                    MarkdownContentUrl = "post.md"
                }
            };
        }
    }
}