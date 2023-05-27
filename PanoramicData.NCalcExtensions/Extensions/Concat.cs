using NCalcAsync;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class Concat
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		var list = new List<object>();
		foreach (var parameter in functionArgs.Parameters)
		{
			var parameterValue = await parameter.EvaluateAsync();
			if (parameterValue is IList parameterValueAsIList)
			{
				foreach (var value in parameterValueAsIList)
				{
					list.Add(value);
				}
			}
			else
			{
				list.Add(parameterValue);
			}
		}

		functionArgs.Result = list;
	}
}