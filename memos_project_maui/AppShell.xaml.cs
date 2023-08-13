using memos_project_maui.Pages;

namespace memos_project_maui;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
		Routing.RegisterRoute(nameof(NewWalkPage), typeof(NewWalkPage));
	}
}
