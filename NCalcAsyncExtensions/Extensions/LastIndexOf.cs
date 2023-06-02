using NCalcAsyncExtensions.Exceptions;
using NCalcAsyncExtensions.Helpers;

namespace NCalcAsyncExtensions.Extensions;

internal static class LastIndexOf
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		try
		{
			var param1 = (string)await functionArgs.Parameters[0].EvaluateSafelyAsync();
			var param2 = (string)await functionArgs.Parameters[1].EvaluateSafelyAsync();
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
