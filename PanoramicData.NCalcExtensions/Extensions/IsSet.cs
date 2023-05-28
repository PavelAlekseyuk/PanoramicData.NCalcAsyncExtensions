namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class IsSet
{
	internal static Task EvaluateAsync(FunctionArgs functionArgs)
	{
		if (functionArgs.Parameters.Length != 1)
		{
			throw new FormatException($"{ExtensionFunction.IsSet}() requires one parameter.");
		}

		functionArgs.Result = functionArgs.Parameters[0].Parameters.Keys.Any(p => p == functionArgs.Parameters[0].EvaluateSafelyAsync().GetAwaiter().GetResult() as string);
		return Task.CompletedTask;
	}
}
