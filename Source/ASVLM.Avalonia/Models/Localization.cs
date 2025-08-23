using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;

using Avalonia.Collections;

using Serilog;

using ASVLM.Common.Libraries;
using ASVLM.CommonAvalonia.Assets.Resources.Localization;
using ASVLM.Avalonia.Managers;

namespace ASVLM.Avalonia.Models;

public class Localization : Model
{
	#region Definitions
	public class Language : Model
	{
		public readonly string culture_code;
		public string Name
		{
			get;
		}
		public string Code
		{
			get;
		}
		public string? Icon
		{
			get;
		}

		public Language(string name, string code, string culture_code, string? icon = null)
		{
			Name = name;
			Code = code;
			this.culture_code = culture_code;
			Icon = icon;
		}
	}
	#endregion
	#region Fields
	private AvaloniaList<Language> _languages;
	private Language _language_text_current, _language_sound_current;
	private readonly DirectoryInfo _localization_directory, _resource_directory;
	private const string _Folder_name_to_change_localization_directory = "text", _Folder_name_to_change_resource_directory = "sound";
	#endregion
	#region Properties
	public AvaloniaList<Language> Languages
	{
		get { return _languages; }
		private set { _languages = value; OnPropertyChanged(); }
	}
	public AvaloniaList<Language> Languages_Text_Current
	{
		get;
	}
	public AvaloniaList<Language> Languages_Sound_Current
	{
		get;
	}
	public Language Language_Text_Current
	{
		get { return _language_text_current; }
		set
		{
			tryChangeLanguage(_language_text_current, value, _localization_directory, _Folder_name_to_change_localization_directory);
			_language_text_current = value;
			OnPropertyChanged();
			AppManager.changeAppLanguage(value.culture_code);
		}
	}
	public Language Language_Sound_Current
	{
		get { return _language_sound_current; }
		set
		{
			tryChangeLanguage(_language_sound_current, value, _resource_directory, _Folder_name_to_change_resource_directory);
			_language_sound_current = value;
			OnPropertyChanged();
		}
	}
	#endregion
	public Localization(string mod_path)
	{
		if (!Directory.Exists(mod_path))
			throw new DirectoryNotFoundException("Mod directory not found!");
		Languages = new() {
			new(nameof(Resources.Language_English_Text), "eng", "en-US", "🇬🇧"),
			new(nameof(Resources.Language_Russian_Text), "rus", "ru-RU", "🇷🇺")
		};
		_localization_directory = new DirectoryInfo($"{mod_path}{Path.DirectorySeparatorChar}localization");
		_resource_directory = new DirectoryInfo($"{mod_path}{Path.DirectorySeparatorChar}resource");
		var language_text = detectLanguage(_localization_directory);
		if(language_text!=null)
			AppManager.changeAppLanguage(language_text.culture_code);
		if (language_text != null)
			Language_Text_Current = language_text;
		var language_sound = detectLanguage(_resource_directory);
		if (language_sound != null)
			Language_Sound_Current = language_sound;
	}
	#region Methods
	public Language? detectLanguage(DirectoryInfo directory_detect_language_in)
	{
		if (!directory_detect_language_in.Exists)
		{
			Log.Error($"Can't detect current language! Directory {directory_detect_language_in.FullName} doesn't exist.");
			return null;
		}
		Dictionary<string, Language> dict_language_code_language = new();
		Regex regex_language_code = new Regex(@".+_([a-z]{3})");

		for (int i = 0; i < Languages.Count; i++)
		{
			dict_language_code_language.Add(Languages[i].Code, Languages[i]);
		}
		foreach (var subdirectory in directory_detect_language_in.GetDirectories())
		{
			Match match = regex_language_code.Match(subdirectory.Name);
			if (dict_language_code_language.ContainsKey(match.Groups[1].Value))
				dict_language_code_language.Remove(match.Groups[1].Value);
		}

		if (dict_language_code_language.Count == 1)
		{
			return dict_language_code_language.First().Value;
		}
		if (dict_language_code_language.Count > 1)
		{
			StringBuilder string_builder_languages_left = new();
			foreach (var language in dict_language_code_language)
			{
				string_builder_languages_left.AppendFormat(" {0},", language.Value.Name);
			}
			string_builder_languages_left.Remove(string_builder_languages_left.Length - 1, 1);
			Log.Error($"Can't detect active language. It either can be:{string_builder_languages_left}.");
			return null;
		}
		else
		{
			StringBuilder string_builder_languages_supported = new();
			foreach (var language in Languages)
			{
				string_builder_languages_supported.AppendFormat(" {0}", language.Name);
			}
			string_builder_languages_supported.Remove(string_builder_languages_supported.Length - 1, 1);
			Log.Error($"Currently active language is not supported. List of supported languages: {string_builder_languages_supported}");
			return null;
		}
	}
	public bool tryChangeLanguage(Language? language_current, Language language_change_to, DirectoryInfo directory, string folder_name_to_change)
	{
		if (language_current == null)
			return false;
		
		string path_directory_language_current = $"{directory.FullName}{Path.DirectorySeparatorChar}{folder_name_to_change}";
		DirectoryInfo directory_language_current = new(path_directory_language_current);
		DirectoryInfo directory_language_change_to = new($"{path_directory_language_current}_{language_change_to.Code}");

		if (!directory.Exists)  //I don't want to create a local function/lambda for these code blocks
		{
			Log.Error($"Can't change language, {directory.FullName} doesn't exist!");
			return false;
		}
		else if (!directory_language_current.Exists)
		{
			Log.Error($"Can't change language, {directory_language_current.FullName} doesn't exist!");
			return false;
		}

		directory_language_current.MoveTo($"{directory_language_current.FullName}_{language_current.Code}");
		directory_language_change_to.MoveTo(path_directory_language_current);
		languageChanged?.Invoke(this, new());

		return true;
	}
	#endregion
	#region Events
	public event EventHandler<EventArgs> languageChanged;
	#endregion
}