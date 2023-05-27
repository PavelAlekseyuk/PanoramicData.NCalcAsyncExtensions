using PanoramicData.NCalcAsyncExtensions.Exceptions;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class LastIndexOf
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		try
		{
			var param1 = (string)functionArgs.Parameters[0].Evaluate();
			var param2 = (string)functionArgs.Parameters[1].Evaluate();
			functionArgs.Result = param1.LastIndexOf(param2, StringComparison.InvariantCulture);
		}
		catch (NCalcExtensionsException)
		{
			throw;
		}
		catch (Exception)
		{
			throw new FormatException($"{ExtensionFunction.LastIndexOf}() requires two string parameters.");
		}
	}
}
