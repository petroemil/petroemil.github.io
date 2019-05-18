using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorSite.BlogService
{
    public class GitHubContent
    {
        public string Name { get; set; }
    }

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
            const string owner = "petroemil";
            const string repo = "petroemil.github.io";
            const string contentDir = "content";
            var gitHubApi = $"https://api.github.com/repos/{owner}/{repo}/contents/{contentDir}";
            var blogPostDirs = await httpClient.GetJsonAsync<GitHubContent[]>(gitHubApi);

#if DEBUG
            blogPostDirs = new[] { new GitHubContent { Name = "test-post" } };
#endif

            var blogPosts = await Task.WhenAll(blogPostDirs.Select(x => GetBlogPost(x.Name)));

            return blogPosts;
        }
    }
}