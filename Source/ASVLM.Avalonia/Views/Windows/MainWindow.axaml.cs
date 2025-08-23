using System;
using System.ComponentModel;
using ASVLM.Avalonia.Managers;
using ASVLM.Avalonia.ViewModels.Windows;
using Avalonia;
using Avalonia.Controls;

namespace ASVLM.Avalonia.Views.Windows;

public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();
		DataContext = new ViewModelMainWindow();
		AppManager.Windows.TryAdd(nameof(ViewModelMainWindow), this);
	}
}