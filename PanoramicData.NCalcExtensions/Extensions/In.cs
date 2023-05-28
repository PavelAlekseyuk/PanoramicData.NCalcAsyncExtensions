using PanoramicData.NCalcAsyncExtensions.Exceptions;
using System.Threading.Tasks;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class In
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		if (functionArgs.Parameters.Length < 2)
		{
			throw new FormatException($"{ExtensionFunction.In}() requires at least two parameters.");
		}

		try
		{
			var item = await functionArgs.Parameters[0].EvaluateAsync();
			var tasks = functionArgs.Parameters.Skip(1).Select(p => p.EvaluateAsync());
			var list = await Task.WhenAll(tasks);
			functionArgs.Result = list.Contains(item);
		}
		catch (NCalcExtensionsException)
		{
			throw;
		}
		catch (Exception)
		{
			throw new FormatException($"{ExtensionFunction.In}() parameters malformed.");
		}
	}
}
