﻿using NCalcAsync;
using PanoramicData.NCalcAsyncExtensions.Exceptions;
using System.Threading.Tasks;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class GetProperty
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		object value;
		string property;
		try
		{
			value = await functionArgs.Parameters[0].EvaluateAsync();
			property = (string)await functionArgs.Parameters[1].EvaluateAsync();
		}
		catch (NCalcExtensionsException)
		{
			throw;
		}
		catch (Exception e)
		{
			throw new FormatException($"{ExtensionFunction.GetProperty}() requires two parameters.", e);
		}

		switch (value)
		{
			case JObject jObject:
				var jToken = jObject[property];

				functionArgs.Result = (jToken?.Type) switch
				{
					null or JTokenType.None or JTokenType.Null or JTokenType.Undefined => null,
					JTokenType.Object => jToken.ToObject<JObject>(),
					JTokenType.Array => jToken.ToObject<JArray>(),
					JTokenType.Constructor => jToken.ToObject<JConstructor>(),
					JTokenType.Property => jToken.ToObject<JProperty>(),
					JTokenType.Comment => jToken.ToObject<JValue>(),
					JTokenType.Integer => jToken.ToObject<int>(),
					JTokenType.Float => jToken.ToObject<float>(),
					JTokenType.String => jToken.ToObject<string>(),
					JTokenType.Boolean => jToken.ToObject<bool>(),
					JTokenType.Date => jToken.ToObject<DateTime>(),
					JTokenType.Raw or JTokenType.Bytes => jToken.ToObject<JValue>(),
					JTokenType.Guid => jToken.ToObject<Guid>(),
					_ => throw new NotSupportedException("Unsupported JTokenType: " + jToken.Type),
				};
				break;
			default:
				var type = value.GetType();
				var propertyInfo = type.GetProperty(property) ?? throw new FormatException($"Could not find property {property} on type {type.Name}");
				functionArgs.Result = propertyInfo.GetValue(value);
				break;
		}
	}
}