using PanoramicData.NCalcAsyncExtensions.Exceptions;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class If
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		bool boolParam1;
		if (functionArgs.Parameters.Length != 3)
		{
			throw new FormatException($"{ExtensionFunction.If}() requires three parameters.");
		}

		try
		{
			boolParam1 = (bool)await functionArgs.Parameters[0].EvaluateSafelyAsync();
		}
		catch (NCalcExtensionsException)
		{
			throw;
		}
		catch (Exception)
		{
			throw new FormatException($"Could not evaluate {ExtensionFunction.If} function parameter 1 '{functionArgs.Parameters[0].ParsedExpression}'.");
		}

		if (boolParam1)
		{
			try
			{
				functionArgs.Result = await functionArgs.Parameters[1].EvaluateSafelyAsync();
				return;
			}
			catch (NCalcExtensionsException)
			{
				throw;
			}
			catch (Exception e)
			{
				throw new FormatException($"Could not evaluate {ExtensionFunction.If} function parameter 2 '{functionArgs.Parameters[1].ParsedExpression}' due to {e.Message}.", e);
			}
		}

		try
		{
			functionArgs.Result = await functionArgs.Parameters[2].EvaluateSafelyAsync();
		}
		catch (NCalcExtensionsException)
		{
			throw;
		}
		catch (Exception e)
		{
			throw new FormatException($"Could not evaluate {ExtensionFunction.If} function parameter 3 '{functionArgs.Parameters[2].ParsedExpression}' due to {e.Message}.", e);
		}
	}
}
