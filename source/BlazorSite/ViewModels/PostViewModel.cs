using BlazorSite.BlogService;
using System.Threading.Tasks;

namespace BlazorSite.ViewModels
{
    public interface IPostViewModelProvider
    {
        Task<PostViewModel> GetViewModel(string postId);
    }

    public class PostViewModelProvider : IPostViewModelProvider
    {
        private readonly IBlogService blogService;

        public PostViewModelProvider(IBlogService blogService)
        {
            this.blogService = blogService;
        }

        public async Task<PostViewModel> GetViewModel(string postId)
        {
            var postDetails = await this.blogService.GetBlogPostDetails(postId, true);
            return new PostViewModel(postDetails);
        }
    }

    public class PostViewModel
    {
        public BlogPostDetails PostDetails { get; }

        public PostViewModel(BlogPostDetails postDetails)
        {
            PostDetails = postDetails;
        }
    }
}
