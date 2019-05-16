using System;
using System.IO;
using CommonMark;
using CommonMark.Formatters;
using CommonMark.Syntax;
using Microsoft.JSInterop;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorSite
{
    public class CustomHtmlFormatter : HtmlFormatter
    {
        private readonly IJSInProcessRuntime jsInterop;

        public CustomHtmlFormatter(IJSInProcessRuntime jsInterop, TextWriter target, CommonMarkSettings settings)
            : base(target, settings)
        {
            this.jsInterop = jsInterop;
        }

        protected override void WriteBlock(Block block, bool isOpening, bool isClosing, out bool ignoreChildNodes)
        {
            if (block.Tag == BlockTag.FencedCode && block.StringContent != null)
            {
                var language = block.FencedCodeData.Info;
                var code = block.StringContent.ToString();

                ignoreChildNodes = true;

                this.Write($"<pre><code class=\"language-{language} hljs\">");

                var formattedCode = jsInterop.Invoke<string>("hljs_interop.highlightSyntax", code);
                this.Write(formattedCode);

                this.Write("</code></pre>");
            }
            else
            {
                base.WriteBlock(block, isOpening, isClosing, out ignoreChildNodes);
            }
        }

        public static void Configure(IServiceProvider services)
        {
            var jsInterop = services.GetService<IJSRuntime>();

            CommonMarkSettings.Default.OutputDelegate = (doc, output, settings) =>
                new CustomHtmlFormatter((IJSInProcessRuntime)jsInterop, output, settings).WriteDocument(doc);
        }
    }
}