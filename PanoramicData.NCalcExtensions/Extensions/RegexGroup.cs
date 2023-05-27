using NCalcAsync;
using PanoramicData.NCalcAsyncExtensions.Exceptions;
using System.Threading.Tasks;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class RegexGroup
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		try
		{
			var input = (string)await functionArgs.Parameters[0].EvaluateAsync();
			var regexExpression = (string)await functionArgs.Parameters[1].EvaluateAsync();
			var regexCaptureIndex = functionArgs.Parameters.Length == 3
				? (int)await functionArgs.Parameters[2].EvaluateAsync()
				: 0;
			var regex = new Regex(regexExpression);
			if (!regex.IsMatch(input))
			{
				functionArgs.Result = null;
			}
			else
			{
				var group = regex
					.Match(input)
					.Groups[1];
				functionArgs.Result = regexCaptureIndex >= group.Captures.Count
					? null
					: group.Captures[regexCaptureIndex].Value;
			}
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
