using System.Collections;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class Take
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		var list = (IList)await functionArgs.Parameters[0].EvaluateSafelyAsync();
		var numberToTake = (int)await functionArgs.Parameters[1].EvaluateSafelyAsync();
		functionArgs.Result = list.Cast<object?>().Take(numberToTake).ToList();
	}
}
