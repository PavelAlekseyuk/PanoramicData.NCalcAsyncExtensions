using NCalcAsyncExtensions.Helpers;
using System.Collections;

namespace NCalcAsyncExtensions.Extensions;

internal static class Skip
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		var list = (IList)await functionArgs.Parameters[0].EvaluateSafelyAsync();
		var numberToSkip = (int)await functionArgs.Parameters[1].EvaluateSafelyAsync();
		functionArgs.Result = list.Cast<object?>().Skip(numberToSkip).ToList();
	}
}
