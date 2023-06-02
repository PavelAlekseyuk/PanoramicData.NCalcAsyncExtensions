using NCalcAsyncExtensions.Exceptions;
using NCalcAsyncExtensions.Helpers;

namespace NCalcAsyncExtensions.Extensions;

internal static class PadLeft
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		string input;
		int desiredStringLength;
		char paddingCharacter;
		try
		{
			input = (string)await functionArgs.Parameters[0].EvaluateSafelyAsync();
			desiredStringLength = (int)await functionArgs.Parameters[1].EvaluateSafelyAsync();
			if (desiredStringLength < 1)
			{
				throw new NCalcExtensionsException($"{ExtensionFunction.PadLeft}() requires a DesiredStringLength for parameter 2 that is >= 1.");
			}

			var paddingString = await functionArgs.Parameters[2].EvaluateSafelyAsync() as string
				?? throw new NCalcExtensionsException($"{ExtensionFunction.PadLeft}() requires that parameter 3 be a string."); ;
			if (paddingString.Length != 1)
			{
				throw new NCalcExtensionsException($"{ExtensionFunction.PadLeft}() requires a single character string for parameter 3.");
			}

			paddingCharacter = paddingString[0];
		}
		catch (NCalcExtensionsException)
		{
			throw;
		}
		catch (Exception)
		{
			throw new FormatException($"{ExtensionFunction.PadLeft}() requires a string Input, an integer DesiredStringLength, and a single Padding character.");
		}

		functionArgs.Result = input.PadLeft(desiredStringLength, paddingCharacter);
	}
}
