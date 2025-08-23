using System;
using System.Runtime.InteropServices;
using System.Globalization;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;

namespace ASVLM.Common.Utilities;

public static class BooleanExtensions
{
	public static unsafe int toInt(this bool value)
	{
		return *(byte*)&value;
	}
}
public static class EnumExtensions
{
	public static string getDescription(this Enum value)
	{
		object[]? attributes = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
		if (attributes.Any())
			return (attributes.First() as DescriptionAttribute).Description;

		TextInfo text_info = CultureInfo.CurrentCulture.TextInfo;        //Replace underscores with spaces if there is no description
		return text_info.ToTitleCase(text_info.ToLower(value.ToString().Replace("_", " ")));
	}
}
public static class StringExtensions
{
	public static int getHashCodeConsistent(this string str)
	{
		unchecked
		{
			int hash1 = 5381;
			int hash2 = hash1;

			for (int i = 0; i < str.Length && str[i] != '\0'; i += 2)
			{
				hash1 = (hash1 << 5) + hash1 ^ str[i];
				if (i == str.Length - 1 || str[i + 1] == '\0')
					break;
				hash2 = (hash2 << 5) + hash2 ^ str[i + 1];
			}

			return hash1 + hash2 * 1566083941;
		}
	}
}
public static class NullExtensions
{
	public static T toNonNullable<T>(this T? value)
	{
		T result = value ?? default;
		return result;
	}
}

public static class IEnumerableExtensions
{
	internal static T[][] ToJaggedArray<T>(this T[,] array_2d)
	{
		int rows_first_index = array_2d.GetLowerBound(0);
		int rows_last_index = array_2d.GetUpperBound(0);
		int rows_count = rows_last_index - rows_first_index + 1;
		int cols_first_index = array_2d.GetLowerBound(1);
		int cols_last_index = array_2d.GetUpperBound(1);
		int cols_count = cols_last_index - cols_first_index + 1;
		T[][] array_jagged = new T[rows_count][];

		for (int i = 0; i < rows_count; i++)
		{
			array_jagged[i] = new T[cols_count];
			for (int j = 0; j < cols_count; j++)
				array_jagged[i][j] = array_2d[i + rows_first_index, j + cols_first_index];
		}

		return array_jagged;
	}
	public static void DisposeElements<T>(this IEnumerable<T> enumerable)
	{
		if (typeof(T).GetInterface(nameof(IDisposable)) != null)
			foreach (IDisposable disposable in enumerable)
				disposable?.Dispose();
	}
}
#if DEBUG
public static class ObjectExtensions
{
	public static long getMemoryAddress(this object obj)
	{
		return GCHandle.ToIntPtr(GCHandle.Alloc(obj, GCHandleType.WeakTrackResurrection)).ToInt64();
	}
}
#endif