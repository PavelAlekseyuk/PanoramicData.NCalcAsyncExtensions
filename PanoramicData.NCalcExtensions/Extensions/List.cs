using System.Threading.Tasks;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class List
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		var tasks = functionArgs.Parameters.Select(p => p.EvaluateAsync());
		functionArgs.Result = (await Task.WhenAll(tasks)).ToList();
	}
}
