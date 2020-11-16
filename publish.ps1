Param(
    [string]$GITHUB_TOKEN
)

$ErrorActionPreference = 'Stop'

# Create the release
dotnet publish .\source -c Release

# Copying the published content to the root (on GitHub Pages, that's publishing)
$rootPath = ".\"
$publishPath = ".\source\BlazorSite\bin\Release\net5.0\publish\wwwroot"

$ignored_root = @(".git", ".github", "source", "content", ".gitattributes", ".gitignore", ".nojekyll", "README.md", "publish.ps1")
$ignored_published = @("content")

Get-ChildItem -Exclude $ignored_root | Remove-Item -Recurse
Get-ChildItem $publishPath -Exclude $ignored_published | Copy-Item -Destination $rootPath -Recurse

# Git setup
git remote set-url origin https://x-access-token:${env:GITHUB_TOKEN}@github.com/${env:GITHUB_REPOSITORY}.git
git config --global user.name "GitHub Action"
git config --global user.email "action@github.com"
git config --global core.autocrlf false

# Publish release
git add .
git commit -m "[Publish]"
git push origin HEAD:master
