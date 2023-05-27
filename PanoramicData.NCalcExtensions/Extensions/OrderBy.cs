﻿using System.Collections.Generic;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class OrderBy
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		var parameterIndex = 0;
		var list = functionArgs.Parameters[parameterIndex++].Evaluate() as IEnumerable<object?>
			?? throw new FormatException($"First {ExtensionFunction.OrderBy} parameter must be an IEnumerable.");

		var predicate = functionArgs.Parameters[parameterIndex++].Evaluate() as string
			?? throw new FormatException($"Second {ExtensionFunction.OrderBy} parameter must be a string.");

		var lambdaString = functionArgs.Parameters[parameterIndex++].Evaluate() as string
			?? throw new FormatException($"Third {ExtensionFunction.OrderBy} parameter must be a string.");

		var lambda = new AsyncLambda(predicate, lambdaString, new());

		IOrderedEnumerable<object?> orderable = list
			.OrderBy(value =>
			{
				var result = lambda.Evaluate(value);
				return result;
			});

		var parameterCount = functionArgs.Parameters.Length;

		while (parameterIndex < parameterCount)
		{
			lambdaString = functionArgs.Parameters[parameterIndex++].Evaluate() as string
				?? throw new FormatException($"{ExtensionFunction.OrderBy} parameter {parameterIndex + 1} must be a string.");
			lambda = new AsyncLambda(predicate, lambdaString, new());
			orderable = orderable
						.ThenBy(value =>
						{
							var result = lambda.Evaluate(value);
							return result;
						});
		}

		functionArgs.Result = orderable.ToList();
	}
}
