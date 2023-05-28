using System.Collections;
using System.Threading.Tasks;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class Skip
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		var list = (IList)await functionArgs.Parameters[0].EvaluateAsync();
		var numberToSkip = (int)await functionArgs.Parameters[1].EvaluateAsync();
		functionArgs.Result = list.Cast<object?>().Skip(numberToSkip).ToList();
	}
}
