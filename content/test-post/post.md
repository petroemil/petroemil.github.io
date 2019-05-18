This content was written in Markdown then converted to HTML on the fly.

## Some C# code

```csharp
public static class TextBlockExtensions
{
    public static void WriteLine(this TextBlock textBlock, object obj)
    {
        var lines = textBlock.Text
            .Split(new string[] { Environment.NewLine }, StringSplitOptions.None)
            .TakeLast(24)
            .Concat(new[] { obj.ToString() });

        textBlock.Text = string.Join(Environment.NewLine, lines);
    }
}
```