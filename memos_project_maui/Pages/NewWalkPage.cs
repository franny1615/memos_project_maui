﻿using CommunityToolkit.Maui.Markup;
using memos_project_maui.Utilities;
using memos_project_maui.ViewModels;
using Microsoft.Maui.Maps;
using static CommunityToolkit.Maui.Markup.GridRowsColumns;
using Maps = Microsoft.Maui.Controls.Maps;

namespace memos_project_maui.Pages;

public class NewWalkPage : ContentPage
{
    private readonly IMainViewModel _mainViewModel;

    private int _durationInSeconds = 0;
    private double _distanceInMiles = 0.0;

    private List<Location> _pathLocations;
    private Maps.Polyline _currentPath = null; 

    private Maps.Map _map = new()
    {
        MapType = MapType.Street,
        IsShowingUser = true,
    };

    private Label _distanceLabel = new()
    {
        Text = "0.0 mi",
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
        Text = "Start",
        FontSize = 20,
        FontAttributes = FontAttributes.Bold,
        BackgroundColor = Constants.PrimaryColor,
        TextColor = Colors.White,
        Margin = new Thickness(8)
    };

	public NewWalkPage(IMainViewModel mainViewModel)
	{
        _mainViewModel = mainViewModel;
		Title = "New Walk";

        _pathLocations = new();

        Shell.SetBackButtonBehavior(this, new BackButtonBehavior
        {
            IconOverride = new FontImageSource
            {
                FontFamily = MaterialFont.Name,
                Glyph = MaterialFont.Arrow_back
            }
        });

        Border durationBorder = UIUtils.DataBorder();
        durationBorder.Content = _durationLabel;

        VerticalStackLayout durationStack = new()
        {
            HorizontalOptions = LayoutOptions.Center,
            Spacing = 4,
            Children =
            {
                UIUtils.DataLabel("Duration"),
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
                UIUtils.DataLabel("Distance"),
                distanceBorder
            }
        };

        Content = new Grid
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
        };

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

    private Action StartStopTapped => new(() =>
    {
        if (_startStopButton.Text == "Start")
        {
            _startStopButton.Text = "Stop";
            _mainViewModel.StartTimer(UpdateDurationLabelWithSeconds);
        }
        else if (_startStopButton.Text == "Stop")
        {
            _startStopButton.Text = "Save";
            _mainViewModel.StopTimer();
        }
        else if (_startStopButton.Text == "Save")
        {
            _mainViewModel.SaveWalk(
                _pathLocations,
                durationInSeconds: _durationInSeconds,
                distanceInMiles: _distanceInMiles);
            Shell.Current.GoToAsync("..");
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
}