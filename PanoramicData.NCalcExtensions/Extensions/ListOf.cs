using PanoramicData.NCalcAsyncExtensions.Helpers;
using System.Collections.Generic;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class ListOf
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		var typeString = await functionArgs.Parameters[0].EvaluateSafelyAsync() as string
			?? throw new FormatException($"First {ExtensionFunction.ListOf} parameter must be a string.");

		var remainingParameters = functionArgs.Parameters.Skip(1).ToArray();
		switch (typeString)
		{
			case "byte":
				functionArgs.Result = await GetListOfAsync<byte>(remainingParameters);
				return;
			case "byte?":
				functionArgs.Result = await GetListOfAsync<byte?>(remainingParameters);
				return;
			case "short":
				functionArgs.Result = await GetListOfAsync<short>(remainingParameters);
				return;
			case "short?":
				functionArgs.Result = await GetListOfAsync<short?>(remainingParameters);
				return;
			case "int":
				functionArgs.Result = await GetListOfAsync<int>(remainingParameters);
				return;
			case "int?":
				functionArgs.Result = await GetListOfAsync<int?>(remainingParameters);
				return;
			case "long":
				functionArgs.Result = await GetListOfAsync<long>(remainingParameters);
				return;
			case "long?":
				functionArgs.Result = await GetListOfAsync<long?>(remainingParameters);
				return;
			case "float":
				functionArgs.Result = await GetListOfAsync<float>(remainingParameters);
				return;
			case "float?":
				functionArgs.Result = await GetListOfAsync<float?>(remainingParameters);
				return;
			case "double":
				functionArgs.Result = await GetListOfAsync<double>(remainingParameters);
				return;
			case "double?":
				functionArgs.Result = await GetListOfAsync<double?>(remainingParameters);
				return;
			case "decimal":
				functionArgs.Result = await GetListOfAsync<decimal>(remainingParameters);
				return;
			case "decimal?":
				functionArgs.Result = await GetListOfAsync<decimal?>(remainingParameters);
				return;
			case "string":
				functionArgs.Result = await GetListOfAsync<string>(remainingParameters);
				return;
			case "string?":
				functionArgs.Result = await GetListOfAsync<string?>(remainingParameters);
				return;
			case "object":
				functionArgs.Result = await GetListOfAsync<object>(remainingParameters);
				return;
			case "object?":
				functionArgs.Result = await GetListOfAsync<object?>(remainingParameters);
				return;
			default:
				throw new FormatException($"First {ExtensionFunction.ListOf} parameter must be a string of a numeric or string type.");
		}
	}

	private static async Task<List<T>> GetListOfAsync<T>(Expression[] remainingParameters)
	{
		var list = new List<T>();
		foreach (var parameter in remainingParameters)
		{
			var value = await parameter.EvaluateSafelyAsync();
			if (typeof(T) == typeof(object))
			{
				list.Add((T)value);
			}
			else if (Nullable.GetUnderlyingType(typeof(T)) != null && value == null)
			{
				list.Add(default!);
			}
			else if (Nullable.GetUnderlyingType(typeof(T)) != null && value != null)
			{
				var underlyingType = Nullable.GetUnderlyingType(typeof(T));
				if (underlyingType != null)
				{
					var convertedValue = Convert.ChangeType(value, underlyingType);
					list.Add((T)convertedValue);
				}
			}
			else if (value is T tValue)
			{
				list.Add(tValue);
			}
			else if (Convert.ChangeType(value, typeof(T)) is T convertedValue)
			{
				list.Add(convertedValue);
			}
			else
			{
				throw new FormatException($"Parameter must be of type {TypeHelper.AsHumanString<T>()}.");
			}
		}

		return list;
	}
}
