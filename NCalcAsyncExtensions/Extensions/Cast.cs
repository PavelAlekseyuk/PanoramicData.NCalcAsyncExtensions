using NCalcAsyncExtensions.Helpers;

namespace NCalcAsyncExtensions.Extensions;

internal static class Cast
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		const int castParameterCount = 2;
		if (functionArgs.Parameters.Length != castParameterCount)
		{
			throw new ArgumentException($"{ExtensionFunction.Cast} function - Expected {castParameterCount} arguments");
		}

		var inputObject = await functionArgs.Parameters[0].EvaluateSafelyAsync();
		if (await functionArgs.Parameters[1].EvaluateSafelyAsync() is not string castTypeString)
		{
			throw new ArgumentException($"{ExtensionFunction.Cast} function - Expected second argument to be a string.");
		}

		var castType = Type.GetType(castTypeString)
			?? throw new ArgumentException($"{ExtensionFunction.Cast} function - Expected second argument to be a valid .NET type e.g. System.Decimal.");

		var result = Convert.ChangeType(inputObject, castType, CultureInfo.InvariantCulture);
		functionArgs.Result = result;
	}
}
