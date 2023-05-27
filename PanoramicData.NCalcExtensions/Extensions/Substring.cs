using NCalcAsync;
using PanoramicData.NCalcAsyncExtensions.Exceptions;
using System.Threading.Tasks;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class Substring
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		string input;
		int startIndex;
		try
		{
			input = (string)await functionArgs.Parameters[0].EvaluateAsync();
			startIndex = (int)await functionArgs.Parameters[1].EvaluateAsync();
		}
		catch (NCalcExtensionsException)
		{
			throw;
		}
		catch (Exception)
		{
			throw new FormatException($"{ExtensionFunction.Substring}() requires a string parameter and one or two numeric parameters.");
		}

		if (functionArgs.Parameters.Length > 2)
		{
			var length = (int)await functionArgs.Parameters[2].EvaluateAsync();
			functionArgs.Result = input.Substring(startIndex, Math.Min(length, input.Length - startIndex));
			return;
		}

		functionArgs.Result = input.Substring(startIndex);
	}
}
