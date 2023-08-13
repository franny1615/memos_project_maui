using CommunityToolkit.Maui.Markup;
using memos_project_maui.Database;
using memos_project_maui.Models;
using memos_project_maui.Utilities;
using System.Collections.ObjectModel;

namespace memos_project_maui.Pages;

public class MainPage : ContentPage
{
	private ObservableCollection<Walk> _walks;
	private readonly IWalkingDatabase _database; 

	public MainPage(IWalkingDatabase database)
	{
		_database = database;
		_walks = new ObservableCollection<Walk>();

		Title = "Walks";

		Content = new Grid()
		{
			Children =
			{
				new CollectionView()
				{
					ItemTemplate = UIUtils.MakeWalkCardTemplate()
				}
				.Bind(CollectionView.ItemsSourceProperty, nameof(_walks))
				.ZIndex(0),
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

	private void AddButtonClicked(object sender, EventArgs e)
	{
		// TODO: should take to a record new walk/jog/run
	}
}