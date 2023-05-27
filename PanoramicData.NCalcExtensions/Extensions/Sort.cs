using NCalcAsync;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class Sort
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		var list = await functionArgs.Parameters[0].EvaluateAsync() as IEnumerable<object?>
			?? throw new FormatException($"First {ExtensionFunction.Sort} parameter must be an IEnumerable.");

		var direction = functionArgs.Parameters.Length > 1
			? await functionArgs.Parameters[1].EvaluateAsync() as string ?? throw new FormatException($"Second {ExtensionFunction.Where} parameter must be a string.")
			: "asc";

		functionArgs.Result = direction.ToUpperInvariant() switch
		{
			"ASC" => list.OrderBy(u => u).ToList(),
			"DESC" => list.OrderByDescending(u => u).ToList(),
			_ => throw new FormatException($"If provided, the second {ExtensionFunction.Sort} parameter must be either 'asc' or 'desc'.")
		};
	}
}
