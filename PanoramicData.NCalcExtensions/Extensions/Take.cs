using System.Collections;
using System.Threading.Tasks;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class Take
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		var list = (IList)functionArgs.Parameters[0].EvaluateAsync();
		var numberToTake = (int)await functionArgs.Parameters[1].EvaluateAsync();
		functionArgs.Result = list.Cast<object?>().Take(numberToTake).ToList();
	}
}
