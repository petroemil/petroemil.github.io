# Create the release
dotnet publish .\source -c Release

# Copying the published content to the root (on GitHub Pages, that's publishing)
$rootPath = ".\"
$publishPath = ".\source\BlazorSite\bin\Release\netstandard2.0\publish\BlazorSite\dist"

$ignored_root = @(".git", "source", "content", ".gitattributes", ".gitignore", ".nojekyll", "README.md", "publish.ps1")
$ignored_published = @("content")

Get-ChildItem -Exclude $ignored_root | Remove-Item -Recurse
Get-ChildItem $publishPath -Exclude $ignored_published | Copy-Item -Destination $rootPath -Recurse

# Publish release
git add .
git commit -m "[Publish]"
git push