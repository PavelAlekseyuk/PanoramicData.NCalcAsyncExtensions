using NCalcAsyncExtensions.Exceptions;
using NCalcAsyncExtensions.Helpers;
using System.Collections;

namespace NCalcAsyncExtensions.Extensions;

internal static class Length
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		try
		{
			var value = await functionArgs.Parameters[0].EvaluateSafelyAsync();
			functionArgs.Result = value switch
			{
				string a => a.Length,
				_ => GetLength(value)
			};
		}
		catch (NCalcExtensionsException)
		{
			throw;
		}
		catch (Exception)
		{
			throw new FormatException($"{ExtensionFunction.Length}() requires one string or IList parameter.");
		}
	}

	private static int GetLength(object value)
	{
		var a = value as IList;
		if (a is not null)
		{
			return a.Count;
		}

		throw new FormatException($"{ExtensionFunction.Length}() requires one string or IList parameter.");
	}
}