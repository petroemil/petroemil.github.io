using System.IO;
using CommonMark;
using CommonMark.Formatters;
using CommonMark.Syntax;

namespace BlazorSite.Markdown
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
    }
}