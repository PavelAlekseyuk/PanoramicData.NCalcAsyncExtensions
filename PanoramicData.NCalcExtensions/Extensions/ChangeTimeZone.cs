using System.Threading.Tasks;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class ChangeTimeZone
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		if (functionArgs.Parameters.Length != 3)
		{
			throw new ArgumentException($"{ExtensionFunction.ChangeTimeZone} function - Expected 3 arguments");
		}

		var argument1 = (await functionArgs.Parameters[0].EvaluateAsync() as DateTime?) ?? throw new ArgumentException($"{ExtensionFunction.ChangeTimeZone} function - parameter 1 should be a DateTime");
		var argument2 = (await functionArgs.Parameters[1].EvaluateAsync() as string) ?? throw new ArgumentException($"{ExtensionFunction.ChangeTimeZone} function - parameter 2 should be a string");
		var argument3 = (await functionArgs.Parameters[2].EvaluateAsync() as string) ?? throw new ArgumentException($"{ExtensionFunction.ChangeTimeZone} function - parameter 3 should be a string");

		functionArgs.Result = ToDateTime.ConvertTimeZone(argument1, argument2, argument3);
	}
}
