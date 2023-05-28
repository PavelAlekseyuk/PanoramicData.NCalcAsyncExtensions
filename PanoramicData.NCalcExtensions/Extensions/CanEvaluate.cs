using System.Threading.Tasks;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class CanEvaluate
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		try
		{
			foreach (var parameter in functionArgs.Parameters)
			{
				await parameter.EvaluateAsync();
			}

			functionArgs.Result = true;
		}
		catch
		{
			functionArgs.Result = false;
		}
	}
}
