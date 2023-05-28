using PanoramicData.NCalcAsyncExtensions.Exceptions;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class EndsWith
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		try
		{
			var param1 = (string)await functionArgs.Parameters[0].EvaluateSafelyAsync();
			var param2 = (string)await functionArgs.Parameters[1].EvaluateSafelyAsync();
			functionArgs.Result = param1.EndsWith(param2, StringComparison.InvariantCulture);
		}
		catch (NCalcExtensionsException)
		{
			throw;
		}
		catch (Exception)
		{
			throw new FormatException($"{ExtensionFunction.EndsWith} function requires two string parameters.");
		}
	}
}
