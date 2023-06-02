using NCalcAsyncExtensions.Exceptions;
using NCalcAsyncExtensions.Helpers;

namespace NCalcAsyncExtensions.Extensions;

internal static class IsNaN
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		if (functionArgs.Parameters.Length != 1)
		{
			throw new FormatException($"{ExtensionFunction.IsNaN}() requires one parameter.");
		}

		try
		{
			var outputObject = await functionArgs.Parameters[0].EvaluateSafelyAsync();
			functionArgs.Result = outputObject is not double || double.IsNaN((double)outputObject);
		}
		catch (NCalcExtensionsException)
		{
			throw;
		}
		catch (FormatException)
		{
			throw;
		}
		catch (Exception e)
		{
			throw new FormatException(e.Message);
		}
	}
}
