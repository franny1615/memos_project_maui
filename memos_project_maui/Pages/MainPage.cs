using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Markup;
using memos_project_maui.Controls;
using memos_project_maui.Database;
using memos_project_maui.Models;
using memos_project_maui.Utilities;
using static CommunityToolkit.Maui.Markup.GridRowsColumns;

namespace memos_project_maui.Pages;

public class MainPage : ContentPage, IQueryAttributable
{
	private ObservableCollection<Walk> _walks;
	private readonly IWalkingDatabase _database;

	private CollectionView _walksDisplay = new()
	{
		Margin = new Thickness(8)
	};

	private NavBar _navBar = new()
	{
		ShowBackButton = false,
		ShowMenuButton = false,
		NavTitle = LanguageManager.Instance["Walks"]
	};

	private Grid _pageContainer = new()
	{
		RowDefinitions = Rows.Define(Auto, Star)
	};

	public MainPage(IWalkingDatabase database)
	{
		Shell.SetNavBarIsVisible(this, false);
		_database = database;
		_walks = new();

		var layout = new GridItemsLayout(ItemsLayoutOrientation.Vertical);
		layout.VerticalItemSpacing = 8;
		_walksDisplay.ItemsLayout = layout;
		_walksDisplay.ItemTemplate = UIUtils.WalkCard(WalkTapped);
		_walksDisplay.ItemsSource = _walks;

		_pageContainer.Children.Add(_navBar.Row(0));
		_pageContainer.Children.Add(new Grid()
        {
            Children =
            {
                _walksDisplay.ZIndex(0),
                UIUtils.CircularButton(
                    AddButtonClicked,
                    glyph: MaterialFont.Add)
                .Margin(new Thickness(0,0,16,16))
                .Bottom()
                .End()
                .ZIndex(1)
            }
        }.Row(1));

        Content = _pageContainer;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		FetchWalks();
    }

	private async void FetchWalks()
	{
		_walks.Clear();
		List<Walk> walks = await _database.GetWalksAsync();
		MainThread.BeginInvokeOnMainThread(() =>
		{
			walks.ForEach(async (walk) =>
			{
				_walks.Add(walk);
				await Task.Delay(40);
			});
		});
    }

	private Action AddButtonClicked => () =>
	{
        Shell.Current.GoToAsync(nameof(WalkPage));
    };

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