using CommunityToolkit.Maui.Markup;
using memos_project_maui.Database;
using static CommunityToolkit.Maui.Markup.GridRowsColumns;
using Maps = Microsoft.Maui.Controls.Maps;

namespace memos_project_maui.Pages;

public class NewWalkPage : ContentPage
{
    private readonly IWalkingDatabase _database; 

    private Maps.Map _map = new()
    {
        MapType = Microsoft.Maui.Maps.MapType.Street
    };

    private Label _distanceLabel = new()
    {
        Text = "Distance 0.0 mi",
        FontSize = 18,
        VerticalOptions = LayoutOptions.Center,
        Margin = new Thickness(8)
    };
    private Label _durationLabel = new()
    {
        Text = "Duration: 00:00:00",
        FontSize = 18,
        VerticalOptions = LayoutOptions.Center,
        Margin = new Thickness(8)
    };
    private Button _startStopButton = new()
    {
        Text = "Start",
        FontSize = 18,
        FontAttributes = FontAttributes.Bold,
        BackgroundColor = Constants.PrimaryColor,
        TextColor = Colors.White,
        Margin = new Thickness(8)
    };

	public NewWalkPage(IWalkingDatabase database)
	{
        _database = database;
		Title = "New Walk";

        Shell.SetBackButtonBehavior(this, new BackButtonBehavior
        {
            IconOverride = new FontImageSource
            {
                FontFamily = MaterialFont.Name,
                Glyph = MaterialFont.Arrow_back
            }
        });

        Content = new Grid
        {
            RowDefinitions = Rows.Define(
                new GridLength(0.7, GridUnitType.Star),
                new GridLength(0.1, GridUnitType.Star),
                new GridLength(0.1, GridUnitType.Star),
                new GridLength(0.1, GridUnitType.Star)
            ),
            Children =
            {
                _map.Row(0),
                _durationLabel.Row(1),
                _distanceLabel.Row(2),
                _startStopButton.TapGesture(StartStopTapped).Row(3)
            }
        };
    }

    private Action StartStopTapped => new(() =>
    {
        if (_startStopButton.Text == "Start")
        {
            _startStopButton.Text = "Stop";
        }
        else if (_startStopButton.Text == "Stop")
        {
            _startStopButton.Text = "Start";
        }
    });
}
