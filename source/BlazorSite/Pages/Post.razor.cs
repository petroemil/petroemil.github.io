using System.Threading.Tasks;
using BlazorSite.ViewModels;
using Microsoft.AspNetCore.Components;

namespace BlazorSite.Pages
{
    public class PostBase : ComponentBase
    {
        [Parameter]
        public string? PostId { get; set; }

        [Inject]
        private IPostViewModelProvider? ViewModelProvider { get; set; }
        protected PostViewModel? ViewModel { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            ViewModel = await ViewModelProvider!.GetViewModel(PostId!);
        }
    }
}