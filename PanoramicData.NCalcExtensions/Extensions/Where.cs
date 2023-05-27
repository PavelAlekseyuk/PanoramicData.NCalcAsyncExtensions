using NCalcAsync;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class Where
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		var list = await functionArgs.Parameters[0].EvaluateAsync() as IEnumerable<object?>
			?? throw new FormatException($"First {ExtensionFunction.Where} parameter must be an IEnumerable.");

		var predicate = await functionArgs.Parameters[1].EvaluateAsync() as string
			?? throw new FormatException($"Second {ExtensionFunction.Where} parameter must be a string.");

		var lambdaString = await functionArgs.Parameters[2].EvaluateAsync() as string
			?? throw new FormatException($"Third {ExtensionFunction.Where} parameter must be a string.");

		var lambda = new AsyncLambda(predicate, lambdaString, new());

		functionArgs.Result = list
			.Where(value =>
			{
				var result = lambda.Evaluate(value) as bool?;
				return result == true;
			})
			.ToList();
	}
}