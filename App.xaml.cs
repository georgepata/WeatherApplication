﻿using WeatherApplication.ViewModel;

namespace WeatherApplication;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}
}

