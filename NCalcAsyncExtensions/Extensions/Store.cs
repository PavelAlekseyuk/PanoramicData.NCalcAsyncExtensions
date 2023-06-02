using NCalcAsyncExtensions.Exceptions;
using NCalcAsyncExtensions.Helpers;
using System.Collections.Generic;

namespace NCalcAsyncExtensions.Extensions;

internal static class Store
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs, Dictionary<string, object?> storageDictionary)
	{
		string key;
		object? value;
		try
		{
			key = (string)await functionArgs.Parameters[0].EvaluateSafelyAsync();
			value = await functionArgs.Parameters[1].EvaluateSafelyAsync();
		}
		catch (NCalcExtensionsException)
		{
			throw;
		}
		catch (Exception)
		{
			throw new FormatException($"{ExtensionFunction.Store}() requires two parameters.");
		}

		storageDictionary[key] = value;

		functionArgs.Result = true;
	}
}
