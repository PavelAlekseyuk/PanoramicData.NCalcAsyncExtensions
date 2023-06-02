using NCalcAsyncExtensions.Exceptions;
using NCalcAsyncExtensions.Helpers;

namespace NCalcAsyncExtensions.Extensions;

internal static class ConvertFunction
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		if (functionArgs.Parameters.Length != 2)
		{
			throw new FormatException($"{ExtensionFunction.Convert}() requires two parameters.");
		}

		try
		{
			// Feed the result of the first parameter into the variables available to the second parameter
			var param1 = await functionArgs.Parameters[0].EvaluateSafelyAsync();
			functionArgs.Parameters[1].Parameters["value"] = param1;
			functionArgs.Result = await functionArgs.Parameters[1].EvaluateSafelyAsync();
		}
		catch (NCalcExtensionsException)
		{
			throw;
		}
		catch (Exception)
		{
		}
	}
}
