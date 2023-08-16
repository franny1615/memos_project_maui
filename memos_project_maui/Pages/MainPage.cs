using CommunityToolkit.Maui.Markup;
using memos_project_maui.Database;
using memos_project_maui.Models;
using memos_project_maui.Utilities;

namespace memos_project_maui.Pages;

public class MainPage : ContentPage, IQueryAttributable
{
	private List<Walk> _walks;
	private readonly IWalkingDatabase _database;

	private CollectionView _walksDisplay = new()
	{
		Margin = new Thickness(8)
	};

	public MainPage(IWalkingDatabase database)
	{
		_database = database;
		_walks = new();

		Title = "Walks";
		_walksDisplay.ItemTemplate = UIUtils.MakeWalkCardTemplate(WalkTapped);

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
		Shell.Current.GoToAsync(nameof(WalkPage));
	}

	private void WalkTapped(object sender)
	{
		Walk pastWalk = (Walk)sender;
		if (pastWalk == null)
			return;

		Dictionary<string, object> parameters = new()
		{
			{ nameof(Walk), pastWalk }
		};

		Shell.Current.GoToAsync(nameof(WalkPage), parameters);
	}

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey(Constants.RefreshWalksKey) &&
			query[Constants.RefreshWalksKey] is bool)
		{
			bool shouldRefresh = (bool)query[Constants.RefreshWalksKey];
			if (shouldRefresh)
				FetchWalks();
		}
    }
}