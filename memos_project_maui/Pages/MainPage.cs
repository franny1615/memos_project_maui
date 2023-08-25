using CommunityToolkit.Maui.Markup;
using System.Collections.ObjectModel;
using memos_project_maui.Database;
using memos_project_maui.Models;
using memos_project_maui.Utilities;

namespace memos_project_maui.Pages;

public class MainPage : BasePage, IQueryAttributable
{
	private ObservableCollection<Walk> _walks;
	private readonly IWalkingDatabase _database;

	private CollectionView _walksDisplay = new()
	{
		Margin = new Thickness(8)
	};

	public MainPage(IWalkingDatabase database) : base()
	{
		_navBar.ShowBackButton = false;
		_navBar.ShowMenuButton = false;
		_navBar.NavTitle = LanguageManager.Instance["Walks"];

        _database = database;
		_walks = new();

		var layout = new GridItemsLayout(ItemsLayoutOrientation.Vertical);
		layout.VerticalItemSpacing = 8;
		_walksDisplay.ItemsLayout = layout;
		_walksDisplay.ItemTemplate = UIUtils.WalkCard(WalkTapped);
		_walksDisplay.ItemsSource = _walks;

		_pageContent.Content = new Grid()
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
        };
        Loaded += MainPage_Loaded;
	}

    private void MainPage_Loaded(object sender, EventArgs e)
    {
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