using CommunityToolkit.Maui.Markup;
using memos_project_maui.Models;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using static CommunityToolkit.Maui.Markup.GridRowsColumns;

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
            Binding id = new("Id");
            Binding seconds = new("DurationInSeconds");
            Binding miles = new("DistanceInMiles");

            Label secondsLabel = new();
            secondsLabel.SetBinding(Label.TextProperty, seconds);

            Label milesLabel = new();
            milesLabel.SetBinding(Label.TextProperty, miles);

            var grid = new Grid
            {
                HeightRequest = 70,
                Padding = 8,
                RowDefinitions = Rows.Define(Star, Star),
                ColumnDefinitions = Columns.Define(Star, Star),
                Children =
                {
                    new Label
                    {
                        Text = "Walk"
                    }
                    .Row(0)
                    .ColumnSpan(2),
                    secondsLabel.Row(1).Column(0),
                    milesLabel.Row(1).Column(1)
                }
            };

            return grid;
        });
    }
}
