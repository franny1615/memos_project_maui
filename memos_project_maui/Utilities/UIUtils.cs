using Microsoft.Maui.Controls.Shapes;

namespace memos_project_maui.Utilities;

public class UIUtils
{
    public static Border MakeCircularButton(
        EventHandler onClick,
        Color backgroundColor,
        Color glyphColor,
        int size = 90)
    {
        Button addButton = new()
        {
            Padding = 0,
            BackgroundColor = Colors.Transparent,
            ImageSource = new FontImageSource()
            {
                FontFamily = MaterialFont.Name,
                Glyph = MaterialFont.Add,
                Size = size / 2,
                Color = glyphColor
            }
        };

        addButton.Clicked += onClick;

        Border circularClipping = new()
        {
            HeightRequest = size,
            WidthRequest = size,
            Stroke = Colors.Transparent,
            StrokeShape = new RoundRectangle()
            {
                CornerRadius = size / 2
            },
            Content = addButton,
            BackgroundColor = backgroundColor
        };

        return circularClipping;
    }

    public static DataTemplate MakeWalkCardTemplate()
    {
        return new DataTemplate(() =>
        {
            var grid = new Grid();

            return grid;
        });
    }
}
