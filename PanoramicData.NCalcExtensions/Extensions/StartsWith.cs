﻿using PanoramicData.NCalcAsyncExtensions.Exceptions;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class StartsWith
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		string param1;
		string param2;
		if (functionArgs.Parameters.Length != 2)
		{
			throw new FormatException($"{ExtensionFunction.StartsWith}() requires two parameters.");
		}

		try
		{
			param1 = (string)await functionArgs.Parameters[0].EvaluateSafelyAsync();
			param2 = (string)await functionArgs.Parameters[1].EvaluateSafelyAsync();
		}
		catch (NCalcExtensionsException)
		{
			throw;
		}
		catch (Exception e)
		{
			throw new FormatException($"Unexpected exception in {ExtensionFunction.StartsWith}(): {e.Message}", e);
		}

		if (param1 == null)
		{
			throw new FormatException($"{ExtensionFunction.StartsWith}() parameter 1 is not a string");
		}

		if (param2 == null)
		{
			throw new FormatException($"{ExtensionFunction.StartsWith}() parameter 2 is not a string");
		}

		functionArgs.Result = param1.StartsWith(param2, StringComparison.InvariantCulture);
	}
}
