using CommonMark;

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

        private CommonMarkSettings CreateSettings(string postId)
        {
            var formatterSettings = CommonMarkSettings.Default.Clone();

            formatterSettings.OutputDelegate = (doc, output, settings) =>
                new CustomMarkdownFormatter(postId, codeHighlighter, output, settings).WriteDocument(doc);

            return formatterSettings;
        }

        public string ConvertMarkdownToHTML(string postId, string markdown)
        {
            var formatterSettingsw = CreateSettings(postId);
            return CommonMarkConverter.Convert(markdown, formatterSettingsw);
        }
    }
}