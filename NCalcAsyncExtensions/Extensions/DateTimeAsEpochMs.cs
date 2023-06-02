using NCalcAsyncExtensions.Helpers;

namespace NCalcAsyncExtensions.Extensions;

internal static class DateTimeAsEpochMs
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		var dateTimeOffset = DateTimeOffset.ParseExact(
			await functionArgs.Parameters[0].EvaluateSafelyAsync() as string, // Input date as string
			await functionArgs.Parameters[1].EvaluateSafelyAsync() as string,
			CultureInfo.InvariantCulture.DateTimeFormat,
			DateTimeStyles.AssumeUniversal);
		functionArgs.Result = dateTimeOffset.ToUnixTimeMilliseconds();
	}
}