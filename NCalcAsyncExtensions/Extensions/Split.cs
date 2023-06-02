using NCalcAsyncExtensions.Exceptions;
using NCalcAsyncExtensions.Helpers;

namespace NCalcAsyncExtensions.Extensions;

internal static class Split
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		string input;
		string splitString;
		try
		{
			input = (string)await functionArgs.Parameters[0].EvaluateSafelyAsync();
			splitString = (string)await functionArgs.Parameters[1].EvaluateSafelyAsync();
		}
		catch (NCalcExtensionsException)
		{
			throw;
		}
		catch (Exception)
		{
			throw new FormatException($"{ExtensionFunction.Split}() requires two string parameters.");
		}

		if (splitString.Length != 1)
		{
			throw new FormatException($"{ExtensionFunction.Split}()'s second parameter must be a single character.");
		}

		functionArgs.Result = input.Split(splitString[0]).ToList();
	}
}