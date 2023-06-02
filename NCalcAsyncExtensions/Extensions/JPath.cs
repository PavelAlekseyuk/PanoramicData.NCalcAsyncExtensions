using NCalcAsyncExtensions.Exceptions;
using NCalcAsyncExtensions.Helpers;

namespace NCalcAsyncExtensions.Extensions;

internal static class JPath
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		JObject jObject;
		string jPathExpression;
		var returnNullIfNotFound = false; // False default

		const string SyntaxMessage = ExtensionFunction.JPath + " function - first parameter should be an object capable of being converted to a JObject and second a string jPath expression with optional third parameter " + nameof(returnNullIfNotFound) + ".";

		if (functionArgs.Parameters.Length > 3)
		{
			throw new FormatException(SyntaxMessage);
		}

		try
		{
			var jPathSourceObject = await functionArgs.Parameters[0].EvaluateSafelyAsync();

			if (jPathSourceObject is null)
			{
				throw new NCalcExtensionsException($"{ExtensionFunction.JPath} function - parameter 1 should not be null.");
			}

			jObject = jPathSourceObject switch
			{
				JObject jObject2 => jObject2,
				_ => JObject.FromObject(jPathSourceObject)
			};

			jPathExpression = (string)await functionArgs.Parameters[1].EvaluateSafelyAsync();
		}
		catch (NCalcExtensionsException)
		{
			throw;
		}
		catch (Exception e)
		{
			throw new FormatException(SyntaxMessage + " ... " + e.Message);
		}

		if (functionArgs.Parameters.Length >= 3)
		{
			try
			{
				returnNullIfNotFound = (bool)await functionArgs.Parameters[2].EvaluateSafelyAsync();
			}
			catch (Exception)
			{
				throw new FormatException($"{ExtensionFunction.JPath} function - parameter 3 should be a bool.");
			}
		}

		JToken? result;
		try
		{
			result = jObject.SelectToken(jPathExpression);
		}
		catch (Exception ex)
		{
			throw new NCalcExtensionsException($"{ExtensionFunction.JPath} function - An unknown issue occurred while trying to select jPathExpression value: {ex.Message}");
		}

		if (result is null)
		{
			if (returnNullIfNotFound)
			{
				functionArgs.Result = null;
				return;
			}
			// Got null, but we didn't ask to returnNullIfNotFound
			throw new NCalcExtensionsException($"{ExtensionFunction.JPath} function - jPath expression did not result in a match.");
		}

		// Try and get the result out of a JValue
		functionArgs.Result = result switch
		{
			JValue jValue => jValue.Value,
			JArray jArray => jArray,
			_ => JObject.FromObject(result)
		};
	}
}
