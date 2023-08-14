using CommunityToolkit.Maui.Markup;
using memos_project_maui.Database;
using memos_project_maui.Models;
using memos_project_maui.Utilities;
using System.Collections.ObjectModel;

namespace memos_project_maui.Pages;

public class MainPage : ContentPage
{
	private List<Walk> _walks;
	private readonly IWalkingDatabase _database;

	private CollectionView _walksDisplay = new()
	{
		ItemTemplate = UIUtils.MakeWalkCardTemplate()
	};

	public MainPage(IWalkingDatabase database)
	{
		_database = database;
		_walks = new();

		Title = "Walks";

        Content = new Grid()
		{
			Children =
			{
				_walksDisplay.ZIndex(0),
				UIUtils.MakeCircularButton(
					AddButtonClicked,
					backgroundColor: Constants.PrimaryColor,
					glyphColor: Colors.White)
				.Margin(new Thickness(0,0,16,16))
				.Bottom()
				.End()
				.ZIndex(1)
			}
		};
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		FetchWalks();
    }

	private async void FetchWalks()
	{
		_walks = await _database.GetWalksAsync();
		MainThread.BeginInvokeOnMainThread(() =>
		{
			_walksDisplay.ItemsSource = _walks;
		});
    }

    private void AddButtonClicked(object sender, EventArgs e)
	{
		Shell.Current.GoToAsync(nameof(NewWalkPage));
	}
}