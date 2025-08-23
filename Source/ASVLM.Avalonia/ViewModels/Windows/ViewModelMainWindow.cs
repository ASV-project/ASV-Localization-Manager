using System;

using ASVLM.Avalonia.Managers;
using ASVLM.Avalonia.Models;
using ASVLM.Common.Libraries;
using ASVLM.CommonAvalonia.Assets.Resources.Localization;

namespace ASVLM.Avalonia.ViewModels.Windows;

public partial class ViewModelMainWindow : Model
{
	#region Fields
	private Localization _localisation;
	private string _message = "";
	private string _select_language;
	private string _select_text_language, _select_voiceover_language;
	private string _app_language_text;
	#endregion
	#region Properties
	public string Select_Localization_Text
	{
		get { return _select_language; }
		set { _select_language = value; OnPropertyChanged(); }
	}
	public string Select_Text_Language_Text
	{
		get { return _select_text_language; }
		set { _select_text_language = value; OnPropertyChanged(); }
	} 
	public string Select_Voiceover_Language_Text
	{
		get { return _select_voiceover_language; }
		set { _select_voiceover_language = value; OnPropertyChanged(); }
	}
	public string App_Language_Text
	{
		get { return _app_language_text; }
		set { _app_language_text = value; OnPropertyChanged(); }
	}
	public string Message
	{
		get { return _message; }
		set { _message = value; OnPropertyChanged(); }
	}
	public Localization Localization
	{
		get { return _localisation; }
		set { _localisation = value; OnPropertyChanged(); }
	}
	#endregion
	public ViewModelMainWindow()
	{
		LoggerErrorSink.onErrorOccured += (sender, e) =>
		{
			Message = e.RenderMessage();
		};
		if (String.IsNullOrEmpty(AppManager.Argument_game_directory_path))
			AppManager.Argument_game_directory_path = Environment.CurrentDirectory;
		_localisation = new(AppManager.Argument_game_directory_path);
		Select_Localization_Text = nameof(Resources.Title_Text);
		Select_Text_Language_Text = nameof(Resources.For_Text_Text);
		Select_Voiceover_Language_Text = nameof(Resources.For_Voiceover_Text);
		App_Language_Text = nameof(Resources.App_Language_Text);
	}
	#region Methods
	
	#endregion
}
