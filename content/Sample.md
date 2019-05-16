# Hello World

This is a markdown file that will get converted to HTML with syntax highlighting for the code.

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

## Some XML code

```xml
<Grid Background="Black">
    <TextBlock x:Name="Console" Margin="25" FontFamily="Consolas" FontSize="24" Foreground="LightGray" IsTextSelectionEnabled="True" />
</Grid>
```