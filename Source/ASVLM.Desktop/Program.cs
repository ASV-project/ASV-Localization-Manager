using System;
using System.IO;

using Avalonia;

using ASVLM.Avalonia.Managers;
using ASVLM.Avalonia.Views;

using Serilog;

namespace ASVLM.Desktop;

internal class Program
{
	public static AppBuilder BuildAvaloniaApp() //Avalonia configuration, don't remove; also used by visual designer.
	{
		return AppBuilder.Configure<App>().UsePlatformDetect().WithInterFont();
	}

	[STAThread] //Don't use any Avalonia, third-party APIs or any SynchronizationContext-reliant code before AppMain is called: things aren't initialized yet and stuff might break.
	public static void Main(string[] args)
	{
		if (args.Length == 1 && (args[0] == "-h" || args[0] == "--help" || args[0] == "-help"))
		{
			Log.Fatal("You can provide explicit --mod_directory <path> arguments.");
			return;
		}
		else
		{
			for (int i = 0, i_original; i < args.Length; i++)
			{
				var throwAndExit = (string argument_name) =>
				{
					Log.Fatal($"Wrong value provided for argument {argument_name}");
					Environment.Exit(1);
				};
				try
				{
					i_original = i;
					switch (args[i])
					{
						case "-mod_directory":
							if (Directory.Exists(args[++i]))
								AppManager.Argument_game_directory_path = args[i];
							else
								throwAndExit(args[i_original]);
							break;
						default:
							Log.Error($"Unknown argument: {args[i_original]}");
							break;
					}
				}
				catch (IndexOutOfRangeException)
				{
					string[] args_tmp = new string[args.Length + 4];
					args.CopyTo(args_tmp, 0);
					for (int ii = args.Length; ii < args_tmp.Length; ii++)
						args_tmp[ii] = "-";
					i--;
				}
			}
		}
		try
		{
			BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
			Log.Information($"Exiting normally.");
		}
		catch (Exception ex)
		{
			Log.Fatal(ex, $"Exiting unexpectendly with the following exception: {ex.Message}.{Environment.NewLine}Stacktrace:{ex.StackTrace}");
		}
		finally
		{
			AppManager.Dispose();
		}
	}
}