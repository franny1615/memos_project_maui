using CommunityToolkit.Maui.Markup;
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

    public static DataTemplate MakeWalkCardTemplate(Action<object> onTap)
    {
        return new DataTemplate(() =>
        {
            Binding Id = new("Id");
            Binding seconds = new("DurationFormatted");
            Binding miles = new("DistanceFormatted");

            Label secondsLabel = new()
            {
                FontSize = 16,
                TextColor = Colors.White
            };
            secondsLabel.SetBinding(Label.TextProperty, seconds);

            Label milesLabel = new()
            {
                FontSize = 16,
                TextColor = Colors.White
            };
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
                        Text = "Walk",
                        FontSize = 18,
                        FontAttributes = FontAttributes.Bold,
                        TextColor = Colors.White
                    }
                    .Row(0)
                    .ColumnSpan(2),
                    secondsLabel.Row(1).Column(0),
                    milesLabel.Row(1).Column(1)
                }
            };

            var border = new Border()
            {
                BackgroundColor = Constants.PrimaryColor,
                Stroke = Colors.Transparent,
                StrokeShape = new RoundRectangle()
                {
                    CornerRadius = 5
                },
                Content = grid
            };

            border.BindingContext = new Binding(".");

            border.TapGesture(() =>
            {
                onTap?.Invoke(border.BindingContext);
            });

            return border;
        });
    }

    public static Border DataBorder()
    {
        return new()
        {
            Stroke = Constants.PrimaryColor,
            StrokeShape = new RoundRectangle()
            {
                CornerRadius = 5
            },
            Padding = 8,
            Margin = 0
        };
    }

    public static Label DataLabel(string text)
    {
        return new()
        {
            Text = text,
            HorizontalTextAlignment = TextAlignment.Center,
            FontSize = 16,
            FontAttributes = FontAttributes.Bold
        };
    }

    public static Button MenuButton(EventHandler clicked)
    {
        FontImageSource menuIcon = new()
        {
            FontFamily = MaterialFont.Name,
            Glyph = MaterialFont.Menu,
            Size = 25,
        };

        menuIcon.SetAppThemeColor(
            FontImageSource.ColorProperty,
            Colors.Black,
            Colors.White);

        return IconButton(menuIcon, clicked);
    }

    public static Button BackButton(EventHandler clicked)
    {
        FontImageSource backIcon = new()
        {
            FontFamily = MaterialFont.Name,
            Glyph = MaterialFont.Arrow_back,
            Size = 25,
        };

        backIcon.SetAppThemeColor(
            FontImageSource.ColorProperty,
            Colors.Black,
            Colors.White);

        return IconButton(backIcon, clicked);
    }

    private static Button IconButton(
        FontImageSource imageSource,
        EventHandler clicked)
    {
        Button button = new()
        {
            ImageSource = imageSource,
            BackgroundColor = Colors.Transparent,
            Padding = 0,
            Margin = 0,
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center
        };
        button.Clicked += clicked;

        return button;
    }
}
