﻿using PanoramicData.NCalcAsyncExtensions.Exceptions;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class IsNaN
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		if (functionArgs.Parameters.Length != 1)
		{
			throw new FormatException($"{ExtensionFunction.IsNaN}() requires one parameter.");
		}

		try
		{
			var outputObject = functionArgs.Parameters[0].Evaluate();
			functionArgs.Result = outputObject is not double || double.IsNaN((double)outputObject);
			return;
		}
		catch (NCalcExtensionsException)
		{
			throw;
		}
		catch (FormatException)
		{
			throw;
		}
		catch (Exception e)
		{
			throw new FormatException(e.Message);
		}
	}
}
