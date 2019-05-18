using System.IO;
using BlazorSite.BlogService;
using CommonMark;
using CommonMark.Formatters;
using CommonMark.Syntax;

namespace BlazorSite.Markdown
{
    public class CustomMarkdownFormatter : HtmlFormatter
    {
        private readonly string postId;
        private readonly ICodeHeighlighter codeHighlighter;

        public CustomMarkdownFormatter(string postId, ICodeHeighlighter codeHighlighter, TextWriter target, CommonMarkSettings settings)
            : base(target, settings)
        {
            this.postId = postId;
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

        protected override void WriteInline(Inline inline, bool isOpening, bool isClosing, out bool ignoreChildNodes)
        {
            if (inline.Tag == InlineTag.Image && !(inline.TargetUrl.StartsWith("http://") || inline.TargetUrl.StartsWith("https://")))
            {
                ignoreChildNodes = true;

                var imgUrl = BlogPostUriHelper.GetContentFileUri(this.postId, inline.TargetUrl);
                var imgAlt = inline.LiteralContent;

                if (isOpening)
                {
                    this.Write($"<img class=\"content-image\" src=\"{imgUrl}\" alt=\"imgAlt\" title=\"{imgAlt}\">");
                }
            }
            else
            {
                base.WriteInline(inline, isOpening, isClosing, out ignoreChildNodes);
            }
        }
    }
}