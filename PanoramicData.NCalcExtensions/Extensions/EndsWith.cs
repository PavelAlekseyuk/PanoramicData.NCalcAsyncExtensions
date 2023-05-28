using PanoramicData.NCalcAsyncExtensions.Exceptions;
using System.Threading.Tasks;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class EndsWith
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		try
		{
			var param1 = (string)await functionArgs.Parameters[0].EvaluateAsync();
			var param2 = (string)await functionArgs.Parameters[1].EvaluateAsync();
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
