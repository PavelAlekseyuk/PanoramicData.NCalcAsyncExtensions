using NCalcAsyncExtensions.Helpers;

namespace NCalcAsyncExtensions.Extensions;

internal static class SetProperties
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		if (functionArgs.Parameters.Length % 2 != 1)
		{
			throw new FormatException($"{ExtensionFunction.NewJObject}() requires an odd number of parameters.");
		}

		var parameterIndex = 0;

		var original = await functionArgs.Parameters[parameterIndex++].EvaluateSafelyAsync();
		var originalAsJObject = original
			switch
			{
				JObject jObject => jObject,
				_ => JObject.FromObject(original)
			};

		// Create an empty JObject
		while (parameterIndex < functionArgs.Parameters.Length)
		{
			if (await functionArgs.Parameters[parameterIndex++].EvaluateSafelyAsync() is not string key)
			{
				throw new FormatException($"{ExtensionFunction.NewJObject}() requires a string key.");
			}

			if (originalAsJObject.ContainsKey(key))
			{
				throw new FormatException($"{ExtensionFunction.NewJObject}() can only define property {key} once.");
			}

			var value = await functionArgs.Parameters[parameterIndex++].EvaluateSafelyAsync();
			originalAsJObject.Add(key, value is null ? JValue.CreateNull() : JToken.FromObject(value));
		}

		functionArgs.Result = originalAsJObject;
	}
}