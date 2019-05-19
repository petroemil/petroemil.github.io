using BlazorSite.BlogService;
using CommonMark;
using CommonMark.Syntax;
using System.Text;

namespace BlazorSite.Markdown
{
    public interface IMarkdownConverter
    {
        string ConvertMarkdownToHTML(string postId, string markdown);
    }

    public class MarkdownConverter : IMarkdownConverter
    {
        private readonly ICodeHeighlighter codeHighlighter;

        public MarkdownConverter(ICodeHeighlighter codeHighlighter)
        {
            this.codeHighlighter = codeHighlighter;
        }

        public string ConvertMarkdownToHTML(string postId, string markdown)
        {
            var formatterSettingsw = CreateSettings(postId);
            return CommonMarkConverter.Convert(markdown, formatterSettingsw);
        }

        private CommonMarkSettings CreateSettings(string postId)
        {
            var formatterSettings = CommonMarkSettings.Default.Clone();

            formatterSettings.OutputDelegate = (doc, output, settings) =>
                new ExtensibleHtmlFormatter(output, settings)
                .WithFormatter(BlockTag.FencedCode, block =>
                {
                    var language = block.FencedCodeData.Info;
                    var code = block.StringContent.ToString();

                    var sb = new StringBuilder();
                    sb.AppendLine($"<pre><code class=\"language-{language} hljs\">");
                    var formattedCode = codeHighlighter.HighlightSyntax(code, language);
                    sb.AppendLine(formattedCode);
                    sb.AppendLine("</code></pre>");

                    return sb.ToString();
                })
                .WithFormatter(InlineTag.Image, inline =>
                {
                    var imgUrl = BlogPostUriHelper.GetContentFileUri(postId, inline.TargetUrl);
                    var imgAlt = inline.LiteralContent;

                    return $"<img class=\"content-image\" src=\"{imgUrl}\" alt=\"imgAlt\" title=\"{imgAlt}\">";
                })
                .WriteDocument(doc);

            return formatterSettings;
        }
    }
}