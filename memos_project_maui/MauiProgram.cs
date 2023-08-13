﻿using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Markup;
using memos_project_maui.Database;
using memos_project_maui.Pages;

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

		// databases
		builder.Services.AddTransient<IWalkingDatabase, WalkingDatabase>();

		return builder.Build();
	}
}
