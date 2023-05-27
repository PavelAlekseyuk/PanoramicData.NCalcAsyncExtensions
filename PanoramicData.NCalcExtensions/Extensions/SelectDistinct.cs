using NCalcAsync;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class SelectDistinct
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		var enumerable = await functionArgs.Parameters[0].EvaluateAsync() as IEnumerable<object?>
			?? throw new FormatException($"First {ExtensionFunction.SelectDistinct} parameter must be an IEnumerable.");

		var predicate = await functionArgs.Parameters[1].EvaluateAsync() as string
			?? throw new FormatException($"Second {ExtensionFunction.SelectDistinct} parameter must be a string.");

		var lambdaString = await functionArgs.Parameters[2].EvaluateAsync() as string
			?? throw new FormatException($"Third {ExtensionFunction.SelectDistinct} parameter must be a string.");

		var lambda = new AsyncLambda(predicate, lambdaString, new());

		functionArgs.Result = enumerable
			.Select(value =>
				{
					var result = lambda.Evaluate(value);
					return result;
				}
			)
			.Distinct()
			.ToList();
	}
}
