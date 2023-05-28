using PanoramicData.NCalcAsyncExtensions.Exceptions;
using System.Threading.Tasks;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class Replace
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		try
		{
			var haystack = (string)await functionArgs.Parameters[0].EvaluateAsync();
			var needle = (string)await functionArgs.Parameters[1].EvaluateAsync();
			var newNeedle = (string)await functionArgs.Parameters[2].EvaluateAsync();
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
