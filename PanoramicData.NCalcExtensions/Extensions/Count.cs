using NCalcAsync;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class Count
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		var list = await functionArgs.Parameters[0].EvaluateAsync() as IEnumerable<object?>
			?? throw new FormatException($"{ExtensionFunction.Count}() requires IEnumerable parameter.");

		if (functionArgs.Parameters.Length == 1)
		{
			functionArgs.Result = list.Count();
			return;
		}

		var predicate = await functionArgs.Parameters[1].EvaluateAsync() as string
			?? throw new FormatException($"Second {ExtensionFunction.Count} parameter must be a string.");

		var lambdaString = await functionArgs.Parameters[2].EvaluateAsync() as string
			?? throw new FormatException($"Third {ExtensionFunction.Count} parameter must be a string.");

		var lambda = new AsyncLambda(predicate, lambdaString, new());
		var results = await Task.WhenAll(list.Select(value => lambda.EvaluateAsync(value)));
		functionArgs.Result = results.Count(result => result as bool? == true);
	}
}