using Microsoft.JSInterop;

namespace BlazorSite.Markdown
{
    public interface ICodeHeighlighter
    {
        string HighlightSyntax(string code, string language);
    }

    public class HighlightJs : ICodeHeighlighter
    {
        private readonly IJSInProcessRuntime jsInterop;

        public HighlightJs(IJSRuntime jsInterop)
        {
            this.jsInterop = (IJSInProcessRuntime)jsInterop;
        }

        public string HighlightSyntax(string code, string language)
        {
            return jsInterop.Invoke<string>("hljs_interop.highlightSyntax", code, new[] { language });
        }
    }
}
