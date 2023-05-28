﻿using PanoramicData.NCalcAsyncExtensions.Exceptions;
using System.Threading.Tasks;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class IsNullOrEmpty
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		if (functionArgs.Parameters.Length != 1)
		{
			throw new FormatException($"{ExtensionFunction.IsNullOrEmpty}() requires one parameter.");
		}

		try
		{
			var outputObject = await functionArgs.Parameters[0].EvaluateAsync();
			functionArgs.Result = outputObject is null ||
				outputObject is JToken { Type: JTokenType.Null } ||
				(outputObject is string outputString && outputString == string.Empty);
		}
		catch (NCalcExtensionsException)
		{
			throw;
		}
		catch (FormatException)
		{
			throw;
		}
		catch (Exception e)
		{
			throw new FormatException(e.Message);
		}
	}
}
