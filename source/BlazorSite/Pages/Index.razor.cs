using BlazorSite.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace BlazorSite.Pages
{
    public class IndexBase : ComponentBase
    {
        [Inject]
        private IIndexViewModelProvider? ViewModelProvider { get; set; }
        protected IndexViewModel? ViewModel { get; private set; }

        protected override async Task OnInitAsync()
        {
            ViewModel = await ViewModelProvider!.GetViewModel();
        }
    }
}