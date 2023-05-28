using System.Collections.Generic;
using System.Threading.Tasks;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class Distinct
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		var enumerable = await functionArgs.Parameters[0].EvaluateAsync() as IEnumerable<object?>
			?? throw new FormatException($"First {ExtensionFunction.Distinct} parameter must be an IEnumerable.");

		functionArgs.Result = enumerable
			.Distinct()
			.ToList();
	}
}
