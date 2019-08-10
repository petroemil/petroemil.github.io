using BlazorSite.BlogService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorSite.ViewModels
{
    public interface IIndexViewModelProvider
    {
        Task<IndexViewModel> GetViewModel();
    }

    public class IndexViewModelProvider : IIndexViewModelProvider
    {
        private readonly IBlogService blogService;

        public IndexViewModelProvider(IBlogService blogService)
        {
            this.blogService = blogService;
        }

        public async Task<IndexViewModel> GetViewModel()
        {
            var blogPosts = await this.blogService.GetBlogPosts();
            return new IndexViewModel(blogPosts);
        }
    }

    public class IndexViewModel
    {
        public IEnumerable<BlogPostDetails> BlogPosts { get; }

        public IndexViewModel(IEnumerable<BlogPostDetails> blogPosts)
        {
            BlogPosts = blogPosts;
        }
    }
}
