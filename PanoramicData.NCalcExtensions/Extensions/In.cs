﻿using PanoramicData.NCalcAsyncExtensions.Exceptions;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class In
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		if (functionArgs.Parameters.Length < 2)
		{
			throw new FormatException($"{ExtensionFunction.In}() requires at least two parameters.");
		}

		try
		{
			var item = functionArgs.Parameters[0].Evaluate();
			var list = functionArgs.Parameters.Skip(1).Select(p => p.Evaluate()).ToList();
			functionArgs.Result = list.Contains(item);
		}
		catch (NCalcExtensionsException)
		{
			throw;
		}
		catch (Exception)
		{
			throw new FormatException($"{ExtensionFunction.In}() parameters malformed.");
		}
	}
}
