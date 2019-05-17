# Experimental Personal Blog

## ASP.NET Blazor

The blog is an ASP.NET (client side) [Blazor](https://github.com/aspnet/Blazor/) site hosted as static content on GitHub Pages.

## CommonMark.NET

All posts are going to be written in markdown that then gets parsed, syntax highlighted and converted to HTML.

The Markdown to HTML parsing is using the [CommonMark.NET](https://github.com/Knagis/CommonMark.NET) library with some custom formatting for code blocks to have syntax highlighting.

## HighlightJS

The custom syntax highlighter is the [HighlightJS](https://highlightjs.org/) library that is being invoked via JavaScript Interop.

## Single Page Apps for GitHub Pages

To enable routing within the SPA, I'm using the [SPA for GitHub "hack"](https://github.com/rafrex/spa-github-pages) from Rafael Pedicini.
