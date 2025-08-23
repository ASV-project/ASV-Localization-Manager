using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ASVLM.Avalonia.Managers;
using ASVLM.Common.Libraries;
using ASVLM.CommonAvalonia.Assets.Resources.Localization;

public class LocalizationManager : Model
{
	public event PropertyChangedEventHandler? PropertyChanged;

	public string this[string key]
	{
		get { return Resources.ResourceManager.GetString(key, Resources.Culture)!; }
	}
	private void notifyStringPropertiesRecursive(Model model)
	{
		Type type = model.GetType();
		PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

		foreach (var property in properties)
		{
			if (typeof(Model).IsAssignableFrom(property.PropertyType))
			{
				notifyStringPropertiesRecursive(property.GetValue(model) as Model);
			}
			else
			{
				if (property.PropertyType == typeof(string) && property.GetIndexParameters().Length <1)
					model.raisePropertyChanged(property.Name);
			}
		}
	}
	public void changeAppLanguage(string culture_code)
	{
		Resources.Culture = new CultureInfo(culture_code);
		CultureChanged?.Invoke(this, EventArgs.Empty);
		foreach (var kv in AppManager.Windows)
		{
			notifyStringPropertiesRecursive(kv.Value.DataContext as Model);
		}
	}
	public static event EventHandler CultureChanged;
}
