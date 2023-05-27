using NCalcAsync;
using PanoramicData.NCalcAsyncExtensions.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class Retrieve
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs, Dictionary<string, object?> storageDictionary)
	{
		string key;
		try
		{
			key = (string)await functionArgs.Parameters[0].EvaluateAsync();
		}
		catch (NCalcExtensionsException)
		{
			throw;
		}
		catch (Exception)
		{
			throw new FormatException($"{ExtensionFunction.Retrieve}() requires one string parameter.");
		}

		functionArgs.Result = storageDictionary.TryGetValue(key, out var value)
			? value
			: throw new FormatException("Key not found");
	}
}