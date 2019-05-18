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

## Some XML code

```xml
<Grid Background="Black">
    <TextBlock x:Name="Console" Margin="25" FontFamily="Consolas" FontSize="24" Foreground="LightGray" IsTextSelectionEnabled="True" />
</Grid>
```

## And some Lorem Ipsum to see paragraph formatting

Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam varius ornare bibendum. Vestibulum in vestibulum mi, at malesuada diam. Donec nulla elit, laoreet vel augue vitae, auctor convallis nulla. Nunc a nulla mi. Curabitur in faucibus elit, maximus vestibulum justo. Aliquam lobortis non lorem nec sollicitudin. Maecenas lacinia tincidunt magna sed venenatis. Mauris sit amet varius ante. Cras dignissim posuere sem, sit amet lacinia lorem finibus id. Suspendisse sed nulla vel urna tempor elementum at in lectus. Vestibulum neque tortor, laoreet a suscipit eu, molestie vel est. Suspendisse nibh arcu, pretium sed aliquam eu, vehicula sed arcu. Phasellus iaculis efficitur diam, ac imperdiet nulla iaculis ac. Aliquam id arcu iaculis quam placerat accumsan a at purus. Aliquam erat volutpat. 

## Image to test URL rewrite and styling

![alt text](images/hololens.png "Minecraft demo with HoloLens")

That's all folks. I think pretty much this is all I need for a meaningful blog engine.
* Text
* Code
* Images