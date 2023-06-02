using NCalcAsyncExtensions.Helpers;
using System.Collections.Generic;

namespace NCalcAsyncExtensions.Extensions;

internal static class LambdaFunction
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs, Dictionary<string, object?> storageDictionary)
	{
		if (functionArgs.Parameters.Length != 2)
		{
			throw new FormatException($"{ExtensionFunction.Store}() requires two parameters.");
		}

		var predicate = await functionArgs.Parameters[0].EvaluateSafelyAsync() as string ?? throw new FormatException($"First {ExtensionFunction.Store} parameter must be an IEnumerable.");;
		var nCalcString = await functionArgs.Parameters[1].EvaluateSafelyAsync() as string ?? throw new FormatException($"Second {ExtensionFunction.Store} parameter must be an IEnumerable.");;;

		functionArgs.Result = new AsyncLambda(predicate, nCalcString, storageDictionary);
	}
}
