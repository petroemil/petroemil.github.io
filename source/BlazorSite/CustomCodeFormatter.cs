using System;
using System.IO;
using CommonMark;
using CommonMark.Formatters;
using CommonMark.Syntax;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorSite
{
    public class CustomCodeFormatter : HtmlFormatter
    {
        private readonly ICodeHeighlighter codeHighlighter;

        public CustomCodeFormatter(ICodeHeighlighter codeHighlighter, TextWriter target, CommonMarkSettings settings)
            : base(target, settings)
        {
            this.codeHighlighter = codeHighlighter;
        }

        protected override void WriteBlock(Block block, bool isOpening, bool isClosing, out bool ignoreChildNodes)
        {
            if (block.Tag == BlockTag.FencedCode && block.StringContent != null)
            {
                ignoreChildNodes = true;

                var language = block.FencedCodeData.Info;
                var code = block.StringContent.ToString();

                this.Write($"<pre><code class=\"language-{language} hljs\">");

                var formattedCode = codeHighlighter.HighlightSyntax(code, language);
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
            var codeHighlighter = services.GetService<ICodeHeighlighter>();

            CommonMarkSettings.Default.OutputDelegate = (doc, output, settings) =>
                new CustomCodeFormatter(codeHighlighter, output, settings).WriteDocument(doc);
        }
    }
}