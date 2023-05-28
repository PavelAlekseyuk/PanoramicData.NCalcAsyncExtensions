using PanoramicData.NCalcAsyncExtensions.Exceptions;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class Replace
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		try
		{
			var haystack = (string)await functionArgs.Parameters[0].EvaluateSafelyAsync();
			var needle = (string)await functionArgs.Parameters[1].EvaluateSafelyAsync();
			var newNeedle = (string)await functionArgs.Parameters[2].EvaluateSafelyAsync();
			functionArgs.Result = haystack.Replace(needle, newNeedle);
		}
		catch (NCalcExtensionsException)
		{
			throw;
		}
		catch (Exception)
		{
			throw new FormatException($"{ExtensionFunction.Replace}() requires three string parameters.");
		}
	}
}
