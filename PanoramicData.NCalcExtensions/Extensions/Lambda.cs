using NCalcAsync;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class LambdaFunction
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs, Dictionary<string, object?> storageDictionary)
	{
		if (functionArgs.Parameters.Length != 2)
		{
			throw new FormatException($"{ExtensionFunction.Store}() requires two parameters.");
		}

		var predicate = await functionArgs.Parameters[0].EvaluateAsync() as string ?? throw new FormatException($"First {ExtensionFunction.Store} parameter must be an IEnumerable.");;
		var nCalcString = await functionArgs.Parameters[1].EvaluateAsync() as string ?? throw new FormatException($"Second {ExtensionFunction.Store} parameter must be an IEnumerable.");;;

		functionArgs.Result = new AsyncLambda(predicate, nCalcString, storageDictionary);
	}
}
