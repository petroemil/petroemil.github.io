using BlazorSite.BlogService.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace BlazorSite.BlogService
{
    public interface IBlogService
    {
        Task<IEnumerable<BlogPostDetails>> GetBlogPosts(bool includeDrafts = false);
        Task<BlogPostDetails> GetBlogPostDetails(string postId, bool detailed);
    }

    public class BlogService : IBlogService
    {
        private readonly HttpClient httpClient;

        public BlogService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<BlogPostDetails> GetBlogPostDetails(string postId, bool detailed)
        {
            var metadataUri = BlogPostUriHelper.GetMetadataFileUri(postId);
            var metadata = await httpClient.GetFromJsonAsync<BlogPostMetadata>(metadataUri);

            var markdownFile = detailed ? metadata.MarkdownFile ?? metadata.SummaryMarkdownFile : metadata.SummaryMarkdownFile;
            var markdownUri = BlogPostUriHelper.GetContentFileUri(postId, markdownFile);
            var markdown = await httpClient.GetStringAsync(markdownUri);

            return new BlogPostDetails(postId, metadata, markdown);
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
                .SelectMany(ids => ids)
                .Select(id => Observable.FromAsync(() => GetBlogPostDetails(id, false)))
                .Merge(10)
                .Where(post => !post.IsDraft || includeDrafts)
                .ToList();

            return posts.OrderByDescending(x => x.PublishDate);
        }
    }
}