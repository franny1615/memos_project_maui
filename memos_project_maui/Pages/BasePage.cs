using CommunityToolkit.Maui.Markup;
using memos_project_maui.Controls;
using static CommunityToolkit.Maui.Markup.GridRowsColumns;

namespace memos_project_maui.Pages;

public class BasePage : ContentPage
{
	internal NavBar _navBar = new();
	internal ContentView _pageContent = new();
	private Grid _pageContainer = new()
	{
		RowDefinitions = Rows.Define(Auto, Star)
	};

	public BasePage()
	{
		Shell.SetNavBarIsVisible(this, false);
		_pageContainer.Children.Add(_navBar.Row(0));
		_pageContainer.Children.Add(_pageContent.Row(1));
		Content = _pageContainer;
	}
}
