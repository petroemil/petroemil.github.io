# Experimental Personal Blog

## ASP.NET Blazor

The blog is an ASP.NET (client side) [Blazor](https://github.com/aspnet/Blazor/) site hosted as static content on GitHub Pages.

## CommonMark.NET for Markdown parsing and HihlightJS for syntax highlighting

All posts are going to be written in markdown that then gets parsed, syntax highlighted and converted to HTML.

The Markdown to HTML parsing is using the [CommonMark.NET](https://github.com/Knagis/CommonMark.NET) library with some custom formatting for code blocks to have syntax highlighting.

The custom syntax highlighter is the [HighlightJS](https://highlightjs.org/) library that is being invoked via JavaScript Interop.

* [Implementation](/source/BlazorSite/Markdown)
* [HighlightJS Helper](/source/BlazorSite/wwwroot/site/js/highlight.interop.js)

## Single Page Apps for GitHub Pages

To enable routing within the SPA, I'm using the [SPA for GitHub "hack"](https://github.com/rafrex/spa-github-pages) from Rafael Pedicini.

* [Implementation](/source/BlazorSite/wwwroot/site/js/gh-spa.js)
* [Index.html](/source/BlazorSite/wwwroot/index.html)
* [404.html](/source/BlazorSite/wwwroot/404.html)

## GitHub Corner

The little animated GitHub link it the top-right corner is cominf from the [GitHub Corner project](https://github.com/tholman/github-corners) by Tim Holman.

## GitHub Actions

GitHub Actions is used to have Continuous Deployment set up if the source code changes.

* [Workflow setup](/.github/workflows/build.yml)
