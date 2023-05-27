using NCalcAsync;
using PanoramicData.NCalcAsyncExtensions.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class Store
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs, Dictionary<string, object?> storageDictionary)
	{
		string key;
		object? value;
		try
		{
			key = (string)await functionArgs.Parameters[0].EvaluateAsync();
			value = await functionArgs.Parameters[1].EvaluateAsync();
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
