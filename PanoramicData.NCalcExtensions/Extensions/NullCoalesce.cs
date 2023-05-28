namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class NullCoalesce
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		foreach (var parameter in functionArgs.Parameters)
		{
			var result = await parameter.EvaluateSafelyAsync();
			if (result is not null)
			{
				functionArgs.Result = result;
				return;
			}
		}

		functionArgs.Result = null;
	}
}
