using PanoramicData.NCalcAsyncExtensions.Exceptions;
using System.Collections;
using System.Threading.Tasks;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class ItemAtIndex
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		IList input;
		int index;
		try
		{
			input = (IList)await functionArgs.Parameters[0].EvaluateAsync();
			index = (int)await functionArgs.Parameters[1].EvaluateAsync();
			if (index < 0)
			{
				throw new Exception();
			}
		}
		catch (NCalcExtensionsException)
		{
			throw;
		}
		catch (Exception)
		{
			throw new FormatException($"{ExtensionFunction.ItemAtIndex}() requires two parameters.  The first should be an IList and the second should be a non-negative integer.");
		}

		functionArgs.Result = input[index];
	}
}
