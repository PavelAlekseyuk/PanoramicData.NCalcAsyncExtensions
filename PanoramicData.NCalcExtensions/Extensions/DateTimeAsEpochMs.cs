using NCalcAsync;
using System.Threading.Tasks;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class DateTimeAsEpochMs
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		var dateTimeOffset = DateTimeOffset.ParseExact(
			await functionArgs.Parameters[0].EvaluateAsync() as string, // Input date as string
			await functionArgs.Parameters[1].EvaluateAsync() as string,
			CultureInfo.InvariantCulture.DateTimeFormat,
			DateTimeStyles.AssumeUniversal);
		functionArgs.Result = dateTimeOffset.ToUnixTimeMilliseconds();
	}
}