using NCalcAsync;
using System.Threading.Tasks;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class List
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
		=> functionArgs.Result = functionArgs.Parameters.Select(p => p.Evaluate()).ToList();
}
