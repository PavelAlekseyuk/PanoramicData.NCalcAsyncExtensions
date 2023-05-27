using PanoramicData.NCalcAsyncExtensions.Exceptions;
using System.Collections.Generic;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class Retrieve
{
	internal static void Evaluate(FunctionArgs functionArgs, Dictionary<string, object?> storageDictionary)
	{
		string key;
		try
		{
			key = (string)functionArgs.Parameters[0].Evaluate();
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