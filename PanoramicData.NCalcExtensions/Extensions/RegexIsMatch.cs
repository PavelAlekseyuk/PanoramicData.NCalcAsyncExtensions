using System.Threading.Tasks;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class RegexIsMatch
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		var input = await functionArgs.Parameters[0].EvaluateAsync();
		var regexExpression = await functionArgs.Parameters[1].EvaluateAsync();
		if (input is not string inputString)
		{
			throw new FormatException($"{ExtensionFunction.RegexIsMatch} function - first parameter should be a string.");
		}

		if (regexExpression is not string regexExpressionString)
		{
			throw new FormatException($"{ExtensionFunction.RegexIsMatch} function - second parameter should be a string.");
		}

		var regex = new Regex(regexExpressionString);
		functionArgs.Result = regex.IsMatch(inputString);
	}
}
