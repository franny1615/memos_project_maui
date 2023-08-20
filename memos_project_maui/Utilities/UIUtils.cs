using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Controls.Shapes;
using static CommunityToolkit.Maui.Markup.GridRowsColumns;

namespace memos_project_maui.Utilities;

public class UIUtils
{
    public static Border CircularButton(
        Action onClick,
        string glyph,
        int size = 75)
    {
        var button = IconButton(MaterialIcon(glyph, size), null);
        button.GestureRecognizers.Clear();

        Border circularClipping = new()
        {
            HeightRequest = size,
            WidthRequest = size,
            Stroke = Colors.Transparent,
            StrokeShape = new RoundRectangle()
            {
                CornerRadius = size / 2
            },
            Content = button,
            BackgroundColor = Constants.PrimaryColor
        };

        circularClipping.TapGesture(async () =>
        {
            await circularClipping.FadeTo(0.5, 100);
            await circularClipping.FadeTo(1, 100);
            onClick?.Invoke();
        });

        return circularClipping;
    }

    public static FontImageSource MaterialIcon(string glyph, int size, bool respondsToTheme = false)
    {
        if (size > 40) // 48px is max for pixel perfect icon
        {
            size = 40;
        }

        var icon = new FontImageSource()
        {
            FontAutoScalingEnabled = false,
            FontFamily = MaterialFont.Name,
            Color = Colors.White,
            Glyph = glyph,
            Size = size
        };

        if (respondsToTheme)
        {
            icon.SetAppThemeColor(FontImageSource.ColorProperty,
                Colors.Black,
                Colors.White);
        }

        return icon;
    }

    public static DataTemplate WalkCard(Action<object> onTap)
    {
        return new DataTemplate(() =>
        {
            Binding Id = new("Id");
            Binding seconds = new("DurationFormatted");
            Binding miles = new("DistanceFormatted");

            Image clockIcon = new()
            {
                Source = MaterialIcon(MaterialFont.Schedule, 24),
                VerticalOptions = LayoutOptions.Center
            };
            Label secondsLabel = new()
            {
                FontSize = 24,
                TextColor = Colors.White,
                VerticalOptions = LayoutOptions.Center
            };
            secondsLabel.SetBinding(Label.TextProperty, seconds);

            Image distanceIcon = new()
            {
                Source = MaterialIcon(MaterialFont.Distance, 24),
                VerticalOptions = LayoutOptions.Center
            };
            Label milesLabel = new()
            {
                FontSize = 24,
                TextColor = Colors.White,
                VerticalOptions = LayoutOptions.Center
            };
            milesLabel.SetBinding(Label.TextProperty, miles);

            var grid = new Grid
            {
                Padding = 8,
                RowDefinitions = Rows.Define(Auto, Star),
                ColumnDefinitions = Columns.Define(
                    Auto,
                    Star,
                    new GridLength(0.05, GridUnitType.Star),
                    Auto,
                    Star),
                ColumnSpacing = 4,
                Children =
                {
                    clockIcon.Row(1).Column(3),
                    distanceIcon.Row(1).Column(0),
                    secondsLabel.Row(1).Column(4),
                    milesLabel.Row(1).Column(1)
                }
            };

            var border = new Border
            {
                BackgroundColor = Constants.PrimaryColor,
                Stroke = Colors.Transparent,
                StrokeShape = new RoundRectangle()
                {
                    CornerRadius = 5
                },
                Content = grid,
                BindingContext = new Binding(".")
            };

            border.TapGesture(() =>
            {
                onTap?.Invoke(border.BindingContext);
            });

            return border;
        });
    }

    public static Image IconButton(
        FontImageSource imageSource,
        Action clicked)
    {
        Image img = new()
        {
            Source = imageSource,
            BackgroundColor = Colors.Transparent,
            Margin = 0,
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center,
        };

        img.TapGesture(async () =>
        {
            await img.FadeTo(0.5, 100);
            await img.FadeTo(1, 100);
            clicked?.Invoke();
        });

        return img;
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
}
