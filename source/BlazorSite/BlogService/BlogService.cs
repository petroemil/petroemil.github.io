using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
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
        public bool IsDraft => PostId.StartsWith("draft-");
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
        Task<IEnumerable<BlogPostMetadata>> GetBlogPosts(bool includeDrafts = false);
        Task<BlogPostMetadata> GetBlogPostDetails(string postId);
    }

    public class BlogService : IBlogService
    {
        private readonly HttpClient httpClient;

        public BlogService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public Task<BlogPostMetadata> GetBlogPostDetails(string postId)
        {
            var metadataFilePath = BlogPostUriHelper.GetMetadataFileUri(postId);
            return httpClient.GetJsonAsync<BlogPostMetadata>(metadataFilePath);
        }

        private async Task<IEnumerable<string>> GetPostIds()
        {
#if DEBUG
            return new[] { "test-post" };
#else
            var hitHubContentUrl = BlogPostUriHelper.GetGitHubContentUrl();
            var postIds = await Observable
                .FromAsync(() => httpClient.GetJsonAsync<GitHubContent[]>(hitHubContentUrl))
                .SelectMany(x => x)
                .Select(x => x.Name)
                .ToList();

            return postIds;
#endif
        }

        public async Task<IEnumerable<BlogPostMetadata>> GetBlogPosts(bool includeDrafts = false)
        {
            var posts = await Observable
                .FromAsync(() => GetPostIds())
                .SelectMany(x => x)
                .Select(x => Observable.FromAsync(() => GetBlogPostDetails(x)))
                .Merge(10)
                .Where(x => !x.IsDraft || includeDrafts)
                .ToList();

            return posts.OrderByDescending(x => x.PublishDate);
        }
    }
}