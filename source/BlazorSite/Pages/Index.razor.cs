using BlazorSite.BlogService;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorSite.Pages
{
    public class IndexBase : ComponentBase
    {
        [Inject]
        private IBlogService BlogService { get; set; }

        protected IEnumerable<BlogPostDetails> BlogPosts { get; set; }

        protected override async Task OnInitAsync()
        {
            BlogPosts = await BlogService.GetBlogPosts();
        }
    }
}