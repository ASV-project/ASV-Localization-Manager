using System;
using System.Linq;

namespace ASVLM.Common.Utilities;

public static class Helper
{
	/// <summary>
	///		Swaps objects.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="first"></param>
	/// <param name="second"></param>
	public static void swap<T>(ref T first, ref T second)
	{
		T temp = first;
		first = second;
		second = temp;
	}
	/// <summary>
	///		Concatenates arrays.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="arrays"></param>
	/// <param name="elements_count_total"></param>
	/// <returns></returns>
	public static T[] concatenateArrays<T>(T[][] arrays, int elements_count_total = 0)
			where T : struct
	{
		int offset = 0;
		T[] result;

		result = elements_count_total <= 0 ? new T[arrays.Sum(arr => arr.Length)] : new T[elements_count_total];
		for (int i = 0; i < arrays.Length; i++)
		{
			Array.Copy(arrays[i], 0, result, offset, arrays[i].Length);
			offset += arrays[i].Length;
		}

		return result;
	}
	public static T[,] To2D<T>(T[][] source)
	{
		try
		{
			int rows_count = source.Length;
			int cols_count = source.GroupBy(row => row.Length).Single().Key; // throws InvalidOperationException if source is not rectangular

			var result = new T[rows_count, cols_count];
			for (int i = 0; i < rows_count; ++i)
				for (int j = 0; j < cols_count; ++j)
					result[i, j] = source[i][j];

			return result;
		}
		catch (InvalidOperationException)
		{
			throw new InvalidOperationException("The given jagged array is not rectangular.");
		}
	}
}