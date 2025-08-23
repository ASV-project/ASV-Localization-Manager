using System;
using System.Collections.Concurrent;

using Avalonia.Controls;

using Serilog;

namespace ASVLM.Avalonia.Managers;

public static class AppManager
{
	#region Fields
	public static string Argument_game_directory_path;
	public static readonly ConcurrentDictionary<string, Window> Windows = new();
	public static LocalizationManager Localization_Manager
	{
		get;
	} = new();
	#endregion
	static AppManager()
	{
		Log.Logger = new LoggerConfiguration()
			.MinimumLevel.Debug()
			.WriteTo.Console()
			.WriteTo.File("Logs/log.txt")
			.WriteTo.Sink(new LoggerErrorSink())
			.CreateLogger();

		Log.Information($"Application starting at {DateTime.Now}.");
	}
	#region Methods
	public static void changeAppLanguage(string culture_code)
	{
		Localization_Manager.changeAppLanguage(culture_code);
	}
	public static void Dispose()
	{
		Log.CloseAndFlush();
	}
	#endregion
}