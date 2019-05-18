using CommonMark;

namespace BlazorSite.Markdown
{
    public interface IMarkdownConverter
    {
        string ConvertMarkdownToHTML(string markdown);
    }

    public class MarkdownConverter : IMarkdownConverter
    {
        private readonly CommonMarkSettings commonMarkSettings;

        public MarkdownConverter(ICodeHeighlighter codeHighlighter)
        {
            this.commonMarkSettings = CommonMarkSettings.Default.Clone();
            this.commonMarkSettings.OutputDelegate = (doc, output, settings) =>
                new CustomCodeFormatter(codeHighlighter, output, settings).WriteDocument(doc);
        }

        public string ConvertMarkdownToHTML(string markdown)
        {
            return CommonMarkConverter.Convert(markdown, this.commonMarkSettings);
        }
    }
}
