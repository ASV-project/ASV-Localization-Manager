using System;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;

using ASVLM.Avalonia.Managers;
using ASVLM.Avalonia.ViewModels.Windows;
using ASVLM.Avalonia.Views.Windows;

namespace ASVLM.Avalonia.Views;

/// <summary>
///		App view.
/// </summary>
public class App : Application  //NOTE To access instance use App.Current
{
	public App()
	{
		DataContext = this;
	}
	#region Methods
	public override void Initialize()
	{
		AvaloniaXamlLoader.Load(this);
	}
	public override void OnFrameworkInitializationCompleted()
	{
		BindingPlugins.DataValidators.RemoveAt(0);

		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
		{
			desktop.MainWindow = new MainWindow();
			desktop.ShutdownMode = ShutdownMode.OnMainWindowClose;
		}
		base.OnFrameworkInitializationCompleted();
	}
	public void exit()
	{
		AppManager.Windows[nameof(ViewModelMainWindow)].Close();    ///Terminate program on MainWindow close <see cref="App.OnFrameworkInitializationCompleted"/>
	}
	#endregion
}