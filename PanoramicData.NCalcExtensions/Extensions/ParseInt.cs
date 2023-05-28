using System.Threading.Tasks;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class ParseInt
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		if (functionArgs.Parameters.Length != 1)
		{
			throw new FormatException($"{ExtensionFunction.ParseInt} function - requires one string parameter.");
		}

		var param1 = await functionArgs.Parameters[0].EvaluateAsync() as string
			?? throw new FormatException($"{ExtensionFunction.ParseInt} function - requires one string parameter.");
		if (!int.TryParse(param1, out var result))
		{
			throw new FormatException($"{ExtensionFunction.ParseInt} function - parameter could not be parsed to an integer.");
		}

		functionArgs.Result = result;
	}
}
