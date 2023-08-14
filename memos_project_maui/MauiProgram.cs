using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Markup;
using memos_project_maui.Database;
using memos_project_maui.Pages;
using memos_project_maui.ViewModels;

namespace memos_project_maui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.UseMauiCommunityToolkitCore()
			.UseMauiCommunityToolkitMarkup()
			.UseMauiMaps()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("material_font.ttf", MaterialFont.Name);
			});

		// pages
		builder.Services.AddTransient<MainPage>();
		builder.Services.AddTransient<NewWalkPage>();

		// databases
		builder.Services.AddTransient<IWalkingDatabase, WalkingDatabase>();

		// viewmodels
		builder.Services.AddTransient<IMainViewModel, MainViewModel>();

		return builder.Build();
	}
}
