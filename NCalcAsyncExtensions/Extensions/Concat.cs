using NCalcAsyncExtensions.Helpers;
using System.Collections;
using System.Collections.Generic;

namespace NCalcAsyncExtensions.Extensions;

internal static class Concat
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		var list = new List<object>();
		foreach (var parameter in functionArgs.Parameters)
		{
			var parameterValue = await parameter.EvaluateSafelyAsync();
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