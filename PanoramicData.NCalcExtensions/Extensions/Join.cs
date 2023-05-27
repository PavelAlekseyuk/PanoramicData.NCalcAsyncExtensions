using NCalcAsync;
using PanoramicData.NCalcAsyncExtensions.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class Join
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		List<string> input;
		string joinString;
		try
		{
			var firstParam = await functionArgs.Parameters[0].EvaluateAsync();
			if (firstParam == null)
			{
				input = new List<string>();
			}
			else if (firstParam is List<object> objList)
			{
				input = objList.Select(u => u.ToString()).ToList();
			}
			else if (firstParam is IEnumerable<string> strEnumerable)
			{
				input = strEnumerable.ToList();
			}
			else
			{
				throw new FormatException($"{ExtensionFunction.Join}() cannot process first parameter of type {firstParam.GetType()}");
			}

			joinString = (string)await functionArgs.Parameters[1].EvaluateAsync();
		}
		catch (NCalcExtensionsException)
		{
			throw;
		}
		catch (Exception)
		{
			throw new FormatException($"{ExtensionFunction.Join}() requires two string parameters.");
		}

		functionArgs.Result = string.Join(joinString, input);
	}
}
