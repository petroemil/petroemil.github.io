var hljs_interop = {

    // This function is needed because the HighlightJS's highlightAuto() function
    // returns an object with circular reference, that Blazor can't process
    highlightSyntax: function (code, languages) {
        return hljs.highlightAuto(code, languages).value;
    }
}