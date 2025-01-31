﻿using NCalcAsyncExtensions.Helpers;
using System.Collections.Generic;

namespace NCalcAsyncExtensions.Extensions;

internal static class Any
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		var list = await functionArgs.Parameters[0].EvaluateSafelyAsync() as IEnumerable<object?>
			?? throw new FormatException($"First {ExtensionFunction.Any} parameter must be an IEnumerable.");

		var predicate = await functionArgs.Parameters[1].EvaluateSafelyAsync() as string
			?? throw new FormatException($"Second {ExtensionFunction.Any} parameter must be a string.");

		var lambdaString = await functionArgs.Parameters[2].EvaluateSafelyAsync() as string
			?? throw new FormatException($"Third {ExtensionFunction.Any} parameter must be a string.");

		var lambda = new AsyncLambda(predicate, lambdaString, new());
		var results = await Task.WhenAll(list.Select(value => lambda.EvaluateAsync(value)));
		functionArgs.Result = results.Any(result => result as bool? == true);
	}
}
