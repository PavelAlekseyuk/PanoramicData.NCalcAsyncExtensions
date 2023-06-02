using NCalcAsyncExtensions.Helpers;
using System.Collections.Generic;

namespace NCalcAsyncExtensions.Extensions;

internal static class Distinct
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		var enumerable = await functionArgs.Parameters[0].EvaluateSafelyAsync() as IEnumerable<object?>
			?? throw new FormatException($"First {ExtensionFunction.Distinct} parameter must be an IEnumerable.");

		functionArgs.Result = enumerable
			.Distinct()
			.ToList();
	}
}
