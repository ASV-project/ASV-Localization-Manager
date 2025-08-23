using System;
using System.Globalization;

using Avalonia.Data.Converters;

using ASVLM.CommonAvalonia.Assets.Resources.Localization;

namespace ASVLM.CommonAvalonia.MVVM.Converters;

/// <summary>
///		Base class for value converters.
/// </summary>
/// <typeparam name="TConverter"></typeparam>
public abstract class ValueConverter<TConverter> : IValueConverter
	where TConverter : class, new()
{
	public static readonly TConverter Instance = new();

	public virtual object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)  //Virtual instead of abstract is intended
	{
		throw new NotSupportedException();
	}
	public virtual object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotSupportedException();
	}
}

public class ConverterStringLocalizedString : ValueConverter<ConverterStringLocalizedString>
{
	public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is string key)
			return Resources.ResourceManager.GetString(key, Resources.Culture)!;
		return string.Empty;
	}
}