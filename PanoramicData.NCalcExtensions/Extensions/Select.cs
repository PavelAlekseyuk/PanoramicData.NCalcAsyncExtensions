using NCalcAsync;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class Select
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		var enumerable = await functionArgs.Parameters[0].EvaluateAsync() as IList
			?? throw new FormatException($"First {ExtensionFunction.Select} parameter must be an IEnumerable.");

		var predicate = await functionArgs.Parameters[1].EvaluateAsync() as string
			?? throw new FormatException($"Second {ExtensionFunction.Select} parameter must be a string.");

		var lambdaString = await functionArgs.Parameters[2].EvaluateAsync() as string
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
