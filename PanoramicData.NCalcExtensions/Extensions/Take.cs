using System.Collections;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class Take
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		var list = (IList)functionArgs.Parameters[0].Evaluate();
		var numberToTake = (int)functionArgs.Parameters[1].Evaluate();
		functionArgs.Result = list.Cast<object?>().Take(numberToTake).ToList();
	}
}
