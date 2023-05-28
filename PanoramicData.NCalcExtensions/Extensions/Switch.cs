using PanoramicData.NCalcAsyncExtensions.Exceptions;
using System.Threading.Tasks;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class Switch
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		if (functionArgs.Parameters.Length < 3)
		{
			throw new FormatException($"{ExtensionFunction.Switch}() requires at least three parameters.");
		}

		object valueParam;
		try
		{
			valueParam = await functionArgs.Parameters[0].EvaluateAsync();
		}
		catch (NCalcExtensionsException)
		{
			throw;
		}
		catch (Exception)
		{
			throw new FormatException($"Could not evaluate {ExtensionFunction.Switch} function parameter 1 '{functionArgs.Parameters[0].ParsedExpression}'.");
		}

		// Determine the pair count
		var pairCount = (functionArgs.Parameters.Length - 1) / 2;
		for (var pairIndex = 0; pairIndex < pairCount * 2; pairIndex += 2)
		{
			var caseIndex = 1 + pairIndex;
			var @case = await functionArgs.Parameters[caseIndex].EvaluateAsync();
			if (@case.Equals(valueParam))
			{
				functionArgs.Result = await functionArgs.Parameters[caseIndex + 1].EvaluateAsync();
				return;
			}
		}

		var defaultIsPresent = functionArgs.Parameters.Length % 2 == 0;
		if (defaultIsPresent)
		{
			functionArgs.Result = await functionArgs.Parameters.Last().EvaluateAsync();
			return;
		}

		throw new FormatException($"Default {ExtensionFunction.Switch} condition occurred, but no default value was specified.");
	}
}
