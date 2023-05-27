﻿using PanoramicData.NCalcAsyncExtensions.Exceptions;
using System.Collections.Generic;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class Store
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs, Dictionary<string, object?> storageDictionary)
	{
		string key;
		object? value;
		try
		{
			key = (string)functionArgs.Parameters[0].Evaluate();
			value = functionArgs.Parameters[1].Evaluate();
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
