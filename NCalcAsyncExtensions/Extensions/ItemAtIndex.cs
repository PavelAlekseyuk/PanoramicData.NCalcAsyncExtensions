﻿using NCalcAsyncExtensions.Exceptions;
using NCalcAsyncExtensions.Helpers;
using System.Collections;

namespace NCalcAsyncExtensions.Extensions;

internal static class ItemAtIndex
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		IList input;
		int index;
		try
		{
			input = (IList)await functionArgs.Parameters[0].EvaluateSafelyAsync();
			index = (int)await functionArgs.Parameters[1].EvaluateSafelyAsync();
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
