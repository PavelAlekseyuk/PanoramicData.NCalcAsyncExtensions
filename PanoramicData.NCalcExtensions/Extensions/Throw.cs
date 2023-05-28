using PanoramicData.NCalcAsyncExtensions.Exceptions;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class Throw
{
	internal static async Task<Exception> EvaluateAsync(FunctionArgs functionArgs)
	{
		switch (functionArgs.Parameters.Length)
		{
			case 0:
				return new NCalcExtensionsException();
			case 1:
				if (await functionArgs.Parameters[0].EvaluateSafelyAsync() is not string exceptionMessageText)
				{
					return new FormatException($"{ExtensionFunction.Throw} function - parameter must be a string.");
				}

				return new NCalcExtensionsException(exceptionMessageText);

			default:
				return new FormatException($"{ExtensionFunction.Throw} function - takes zero or one parameters.");
		}
	}
}
