using System.Collections.Generic;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class OrderBy
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		var parameterIndex = 0;
		var list = await functionArgs.Parameters[parameterIndex++].EvaluateSafelyAsync() as IEnumerable<object?>
			?? throw new FormatException($"First {ExtensionFunction.OrderBy} parameter must be an IEnumerable.");

		var predicate = await functionArgs.Parameters[parameterIndex++].EvaluateSafelyAsync() as string
			?? throw new FormatException($"Second {ExtensionFunction.OrderBy} parameter must be a string.");

		var lambdaString = await functionArgs.Parameters[parameterIndex++].EvaluateSafelyAsync() as string
			?? throw new FormatException($"Third {ExtensionFunction.OrderBy} parameter must be a string.");

		var orderByLambda = new AsyncLambda(predicate, lambdaString, new());
		var orderByResults = await Task.WhenAll(list.Select(value => orderByLambda.EvaluateAsync(value)));
		IOrderedEnumerable<object?> orderable = orderByResults.OrderBy(value => value);

		while (parameterIndex < functionArgs.Parameters.Length)
		{
			lambdaString = await functionArgs.Parameters[parameterIndex++].EvaluateSafelyAsync() as string
				?? throw new FormatException($"{ExtensionFunction.OrderBy} parameter {parameterIndex + 1} must be a string.");
			var thenBylambda = new AsyncLambda(predicate, lambdaString, new());
			orderable = orderable.ThenBy(
				value =>
				{
					var result = thenBylambda.EvaluateAsync(value).GetAwaiter().GetResult();
					return result;
				});
		}

		functionArgs.Result = orderable.ToList();
	}
}
