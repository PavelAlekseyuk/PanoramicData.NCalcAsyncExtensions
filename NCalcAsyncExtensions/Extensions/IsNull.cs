using NCalcAsyncExtensions.Exceptions;
using NCalcAsyncExtensions.Helpers;

namespace NCalcAsyncExtensions.Extensions;

internal static class IsNull
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		if (functionArgs.Parameters.Length != 1)
		{
			throw new FormatException($"{ExtensionFunction.IsNull}() requires one parameter.");
		}

		try
		{
			var outputObject = await functionArgs.Parameters[0].EvaluateSafelyAsync();
			functionArgs.Result = outputObject is null || outputObject is JToken { Type: JTokenType.Null };
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
