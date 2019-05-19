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
        public string HeroImageFile { get; set; }
        public string MarkdownFile { get; set; }
    }

    public static class BlogPostUriHelper
    {
        const string owner = "petroemil";
        const string repo = "petroemil.github.io";
        const string contentRoot = "content";
        const string metadataFile = "metadata.json";

        public static string GetGitHubContentUrl()
            => $"https://api.github.com/repos/{owner}/{repo}/contents/{contentRoot}";

        public static string GetContentFileUri(string postId, string contentFile)
            => $"{contentRoot}/{postId}/{contentFile}";

        public static string GetMetadataFileUri(string postId)
            => GetContentFileUri(postId, metadataFile);
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
            var metadataFilePath = BlogPostUriHelper.GetMetadataFileUri(postId);
            return httpClient.GetJsonAsync<BlogPostMetadata>(metadataFilePath);
        }

        public async Task<IEnumerable<BlogPostMetadata>> GetBlogPosts()
        {
            var hitHubContentUrl = BlogPostUriHelper.GetGitHubContentUrl();
            var blogPostDirs = await httpClient.GetJsonAsync<GitHubContent[]>(hitHubContentUrl);

#if DEBUG
            blogPostDirs = new[] { new GitHubContent { Name = "test-post" } };
#endif

            var blogPosts = await Task.WhenAll(blogPostDirs.Select(x => GetBlogPost(x.Name)));
            return blogPosts.OrderByDescending(x => x.PublishDate);
        }
    }
}