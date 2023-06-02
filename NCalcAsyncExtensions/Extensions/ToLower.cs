using NCalcAsyncExtensions.Exceptions;
using NCalcAsyncExtensions.Helpers;

namespace NCalcAsyncExtensions.Extensions;

internal static class ToLower
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		try
		{
			var param1 = (string)await functionArgs.Parameters[0].EvaluateSafelyAsync();
			functionArgs.Result = param1.ToLowerInvariant();
		}
		catch (NCalcExtensionsException)
		{
			throw;
		}
		catch (Exception)
		{
			throw new FormatException($"{ExtensionFunction.ToLower} function -  requires one string parameter.");
		}
	}
}
