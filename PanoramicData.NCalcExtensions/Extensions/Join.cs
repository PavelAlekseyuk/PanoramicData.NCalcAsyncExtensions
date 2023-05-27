﻿using PanoramicData.NCalcAsyncExtensions.Exceptions;
using System.Collections.Generic;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class Join
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		List<string> input;
		string joinString;
		try
		{
			var firstParam = functionArgs.Parameters[0].Evaluate();
			if (firstParam == null)
			{
				input = new List<string>();
			}
			else if (firstParam is List<object> objList)
			{
				input = objList.Select(u => u.ToString()).ToList();
			}
			else
			{
				input = (firstParam as IEnumerable<string>).ToList();
			}

			joinString = (string)functionArgs.Parameters[1].Evaluate();
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
