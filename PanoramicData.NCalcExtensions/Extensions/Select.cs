using System.Collections;
using System.Collections.Generic;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class Select
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		var enumerable = await functionArgs.Parameters[0].EvaluateSafelyAsync() as IList
			?? throw new FormatException($"First {ExtensionFunction.Select} parameter must be an IEnumerable.");

		var predicate = await functionArgs.Parameters[1].EvaluateSafelyAsync() as string
			?? throw new FormatException($"Second {ExtensionFunction.Select} parameter must be a string.");

		var lambdaString = await functionArgs.Parameters[2].EvaluateSafelyAsync() as string
			?? throw new FormatException($"Third {ExtensionFunction.Select} parameter must be a string.");

		var lambda = new AsyncLambda(predicate, lambdaString, new());

		var result = new List<object?>();
		foreach (var value in enumerable)
		{
			result.Add(await lambda.EvaluateAsync(value));
		}

		functionArgs.Result = result;
	}
}
