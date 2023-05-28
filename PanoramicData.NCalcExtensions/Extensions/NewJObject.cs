using System.Threading.Tasks;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class NewJObject
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		if (functionArgs.Parameters.Length % 2 != 0)
		{
			throw new FormatException($"{ExtensionFunction.NewJObject}() requires an even number of parameters.");
		}

		var parameterIndex = 0;

		// Create an empty JObject
		var jObject = new JObject();
		while (parameterIndex < functionArgs.Parameters.Length)
		{
			if (await functionArgs.Parameters[parameterIndex++].EvaluateAsync() is not string key)
			{
				throw new FormatException($"{ExtensionFunction.NewJObject}() requires a string key.");
			}

			if (jObject.ContainsKey(key))
			{
				throw new FormatException($"{ExtensionFunction.NewJObject}() can only define property {key} once.");
			}

			var value = await functionArgs.Parameters[parameterIndex++].EvaluateAsync();
			jObject.Add(key, value is null ? JValue.CreateNull() : JToken.FromObject(value));
		}

		functionArgs.Result = jObject;
	}
}