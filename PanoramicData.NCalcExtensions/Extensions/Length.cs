using PanoramicData.NCalcAsyncExtensions.Exceptions;
using System.Collections;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class Length
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		try
		{
			var value = functionArgs.Parameters[0].Evaluate();
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