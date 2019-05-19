namespace BlazorSite.BlogService
{
    public static class BlogPostUriHelper
    {
        const string owner = "petroemil";
        const string repo = "petroemil.github.io";
        const string contentRoot = "content";
        const string metadataFile = "metadata.json";

        public static string GetGitHubContentUrl()
            => $"https://api.github.com/repos/{owner}/{repo}/contents/{contentRoot}";

        public static string GetContentFileUri(string postId, string contentFile)
            => $"{contentRoot}/{postId}/{contentFile}";

        public static string GetMetadataFileUri(string postId)
            => GetContentFileUri(postId, metadataFile);
    }
}