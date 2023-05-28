using PanoramicData.NCalcAsyncExtensions.Exceptions;

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
			var item = await functionArgs.Parameters[0].EvaluateSafelyAsync();
			var tasks = functionArgs.Parameters.Skip(1).Select(p => p.EvaluateAsync());
			var list = await Task.WhenAll(tasks).ConfigureAwait(false);
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
