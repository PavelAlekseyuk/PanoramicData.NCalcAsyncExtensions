﻿using NCalcAsync;
using PanoramicData.NCalcAsyncExtensions.Exceptions;
using System.Threading.Tasks;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class ToUpper
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		string param1;
		try
		{
			param1 = (string)await functionArgs.Parameters[0].EvaluateAsync();
			functionArgs.Result = param1.ToUpperInvariant();
		}
		catch (NCalcExtensionsException)
		{
			throw;
		}
		catch (Exception)
		{
			throw new FormatException($"{ExtensionFunction.ToUpper} function -  requires one string parameter.");
		}
	}
}
