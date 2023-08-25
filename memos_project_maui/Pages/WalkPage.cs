using CommunityToolkit.Maui.Markup;
using memos_project_maui.Controls;
using memos_project_maui.Models;
using memos_project_maui.Utilities;
using memos_project_maui.ViewModels;
using Microsoft.Maui.Maps;
using static CommunityToolkit.Maui.Markup.GridRowsColumns;
using Maps = Microsoft.Maui.Controls.Maps;

namespace memos_project_maui.Pages;

public class WalkPage : ContentPage, IQueryAttributable
{
    private readonly IMainViewModel _mainViewModel;

    private int _durationInSeconds = 0;
    private double _distanceInMiles = 0.0;

    private List<Location> _pathLocations;
    private Maps.Polyline _currentPath = null;

    private Walk _pastWalk;

    private NavBar _navBar = new()
    {
        ShowBackButton = true,
        ShowMenuButton = false,
        NavTitle = LanguageManager.Instance["NewWalk"]
    };

    private Grid _pageContainer = new()
    {
        RowDefinitions = Rows.Define(Auto, Star)
    };

    private Maps.Map _map = new()
    {
        MapType = MapType.Street,
        IsShowingUser = true,
    };

    private Label _distanceLabel = new()
    {
        Text = "0 mi",
        FontSize = 24,
        VerticalOptions = LayoutOptions.Center,
        HorizontalTextAlignment = TextAlignment.Center,
        Margin = new Thickness(8)
    };
    private Label _durationLabel = new()
    {
        Text = "00:00:00",
        FontSize = 32,
        VerticalOptions = LayoutOptions.Center,
        HorizontalTextAlignment = TextAlignment.Center,
        Margin = new Thickness(8)
    };
    private Button _startStopButton = new()
    {
        Text = LanguageManager.Instance["Start"],
        FontSize = 20,
        FontAttributes = FontAttributes.Bold,
        BackgroundColor = Constants.PrimaryColor,
        TextColor = Colors.White,
        Margin = new Thickness(8)
    };

	public WalkPage(IMainViewModel mainViewModel)
	{
        Shell.SetNavBarIsVisible(this, false);
        BindingContext = this;
        _mainViewModel = mainViewModel;

        _pathLocations = new();

        Border durationBorder = UIUtils.DataBorder();
        durationBorder.Content = _durationLabel;

        VerticalStackLayout durationStack = new()
        {
            HorizontalOptions = LayoutOptions.Center,
            Spacing = 4,
            Children =
            {
                UIUtils.DataLabel(LanguageManager.Instance["Duration"]),
                durationBorder
            }
        };

        Border distanceBorder = UIUtils.DataBorder();
        distanceBorder.Content = _distanceLabel;

        VerticalStackLayout distanceStack = new()
        {
            HorizontalOptions = LayoutOptions.Center,
            Spacing = 4,
            Children =
            {
                UIUtils.DataLabel(LanguageManager.Instance["Distance"]),
                distanceBorder
            }
        };

        _pageContainer.Children.Add(_navBar.Row(0));
        _pageContainer.Children.Add(new Grid
        {
            RowDefinitions = Rows.Define(
                new GridLength(0.65, GridUnitType.Star),
                Auto,
                Auto,
                new GridLength(0.1, GridUnitType.Star)
            ),
            RowSpacing = 8,
            Children =
            {
                _map.Row(0),
                durationStack.Row(1),
                distanceStack.Row(2),
                _startStopButton.TapGesture(StartStopTapped).Row(3)
            }
        }.Row(1));

        Content = _pageContainer;

        UpdateLocation();
    }

    private async void UpdateLocation()
    {
        Location location = await _mainViewModel.GetCurrentLocation();
        MapSpan span = new MapSpan(location, 0.01, 0.01);

        _pathLocations.Add(location);

        MainThread.BeginInvokeOnMainThread(() =>
        {
            _map.MoveToRegion(span);
        });
    }

    private Action StartStopTapped => new(async () =>
    {
        if (_startStopButton.Text == LanguageManager.Instance["Start"])
        {
            _startStopButton.Text = LanguageManager.Instance["Stop"];

            UpdateLocation();
            DrawPath();

            _mainViewModel.StartWalk(UpdateDurationLabelWithSeconds);
        }
        else if (_startStopButton.Text == LanguageManager.Instance["Stop"])
        {
            _startStopButton.Text = LanguageManager.Instance["Save"];
            _mainViewModel.EndWalk();
        }
        else if (_startStopButton.Text == LanguageManager.Instance["Save"])
        {
            await _mainViewModel.SaveWalk(
                _pathLocations,
                durationInSeconds: _durationInSeconds,
                distanceInMiles: _distanceInMiles);
            Dismiss();
        }
        else if (_startStopButton.Text == LanguageManager.Instance["Delete"])
        {
            await _mainViewModel.DeleteWalk(_pastWalk);
            Dismiss();
        }
    });

    private void UpdateDurationLabelWithSeconds(int duration)
    {
        TimeSpan span = TimeSpan.FromSeconds(duration);
        _durationLabel.Text = span.ToString(@"hh\:mm\:ss");
        _durationInSeconds = duration;

        if (duration % 15 == 0) // every x seconds ping location
        {
            UpdateLocation();
            DrawPath();
        }
    }

    private void DrawPath()
    {
        if (_currentPath != null)
        {
            _map.MapElements.Remove(_currentPath);
        }

        var newPath = new Maps.Polyline
        {
            StrokeColor = Constants.ThirdColor,
            StrokeWidth = 8
        };

        double distance = 0.0;
        Location current = _pathLocations.First();
        newPath.Geopath.Add(_pathLocations.First());
        for(int i = 1; i < _pathLocations.Count; i++)
        {
            double miles = Location.CalculateDistance(
                current,
                _pathLocations[i],
                DistanceUnits.Miles);
            distance += miles;
            current = _pathLocations[i];
            newPath.Geopath.Add(_pathLocations[i]);
        }

        _distanceInMiles = Math.Round(distance, 2);
        _distanceLabel.Text = $"{_distanceInMiles} mi";
        _map.MapElements.Add(newPath);
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey(nameof(Walk)))
        {
            _pastWalk = (Walk)query[nameof(Walk)];
            SetupPastWalkData();
        }
    }

    private async void SetupPastWalkData()
    {
        _navBar.NavTitle = LanguageManager.Instance["PastWalk"];
        _distanceLabel.Text = $"{Math.Round(_pastWalk.DistanceInMiles, 2)} mi";

        TimeSpan span = TimeSpan.FromSeconds(_pastWalk.DurationInSeconds);
        _durationLabel.Text = span.ToString(@"hh\:mm\:ss");

        List<Location> pastPolylinePoints = await _mainViewModel.GetLocationsOfWalkAsync(_pastWalk);

        var pastPath = new Maps.Polyline
        {
            StrokeColor = Constants.ThirdColor,
            StrokeWidth = 8
        };

        pastPolylinePoints.ForEach(pastPath.Geopath.Add);
        _map.MapElements.Add(pastPath);

        _startStopButton.Text = LanguageManager.Instance["Delete"];
    }

    private void Dismiss()
    {
        Dictionary<string, object> parameters = new()
        {
            { Constants.RefreshWalksKey, true }
        };

        Shell.Current.GoToAsync("..", parameters);
    }
}
