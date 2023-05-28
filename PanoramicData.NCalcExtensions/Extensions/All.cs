using System.Collections.Generic;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class All
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		var list = await functionArgs.Parameters[0].EvaluateSafelyAsync() as IEnumerable<object?>
			?? throw new FormatException($"First {ExtensionFunction.All} parameter must be an IEnumerable.");

		var predicate = await functionArgs.Parameters[1].EvaluateSafelyAsync() as string
			?? throw new FormatException($"Second {ExtensionFunction.All} parameter must be a string.");

		var lambdaString = await functionArgs.Parameters[2].EvaluateSafelyAsync() as string
			?? throw new FormatException($"Third {ExtensionFunction.All} parameter must be a string.");

		var lambda = new AsyncLambda(predicate, lambdaString, new(0));
		var results = await Task.WhenAll(list.Select(value => lambda.EvaluateAsync(value)));
		functionArgs.Result = results.All(result => result as bool? == true);
	}
}
