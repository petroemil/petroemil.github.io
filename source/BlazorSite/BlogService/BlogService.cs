using BlazorSite.BlogService.Dto;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace BlazorSite.BlogService
{
    public interface IBlogService
    {
        Task<IEnumerable<BlogPostDetails>> GetBlogPosts(bool includeDrafts = false);
        Task<BlogPostDetails> GetBlogPostDetails(string postId);
    }

    public class BlogService : IBlogService
    {
        private readonly HttpClient httpClient;

        public BlogService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<BlogPostDetails> GetBlogPostDetails(string postId)
        {
            var metadataFilePath = BlogPostUriHelper.GetMetadataFileUri(postId);
            var metadata = await httpClient.GetJsonAsync<BlogPostMetadata>(metadataFilePath);
            return new BlogPostDetails(postId, metadata);
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

        public async Task<IEnumerable<BlogPostDetails>> GetBlogPosts(bool includeDrafts = false)
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