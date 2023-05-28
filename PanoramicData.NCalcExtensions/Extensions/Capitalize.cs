using PanoramicData.NCalcAsyncExtensions.Exceptions;
using PanoramicData.NCalcAsyncExtensions.Helpers;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class Capitalize
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		string param1;
		try
		{
			param1 = (string)await functionArgs.Parameters[0].EvaluateSafelyAsync();
		}
		catch (NCalcExtensionsException)
		{
			throw;
		}
		catch (Exception)
		{
			throw new FormatException($"{ExtensionFunction.Capitalize} function -  requires one string parameter.");
		}

		functionArgs.Result = param1.ToLowerInvariant().UpperCaseFirst();
	}
}
