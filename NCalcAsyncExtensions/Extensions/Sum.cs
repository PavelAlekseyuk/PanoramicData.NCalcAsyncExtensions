using NCalcAsyncExtensions.Helpers;
using System.Collections.Generic;

namespace NCalcAsyncExtensions.Extensions;

internal static class Sum
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		var originalList = await functionArgs.Parameters[0].EvaluateSafelyAsync();

		if (functionArgs.Parameters.Length == 1)
		{
			functionArgs.Result = originalList switch
			{
				IEnumerable<byte> list => list.Cast<int>().Sum(),
				IEnumerable<short> list => list.Cast<int>().Sum(),
				IEnumerable<int> list => list.Sum(),
				IEnumerable<long> list => list.Sum(),
				IEnumerable<float> list => list.Sum(),
				IEnumerable<double> list => list.Sum(),
				IEnumerable<decimal> list => list.Sum(),
				IEnumerable<object?> list => GetSum(list),
				_ => throw new FormatException($"First {ExtensionFunction.Sum} parameter must be an IEnumerable of a numeric type if only on parameter is present.")
			};
			return;
		}

		var predicate = await functionArgs.Parameters[1].EvaluateSafelyAsync() as string
			?? throw new FormatException($"Second {ExtensionFunction.Sum} parameter must be a string.");

		var lambdaString = await functionArgs.Parameters[2].EvaluateSafelyAsync() as string
			?? throw new FormatException($"Third {ExtensionFunction.Sum} parameter must be a string.");

		var lambda = new AsyncLambda(predicate, lambdaString, new());

		functionArgs.Result = originalList switch
		{
			IEnumerable<byte> byteList => byteList.Cast<int>().Sum(value => (int?)lambda.EvaluateAsync(value).GetAwaiter().GetResult()),
			IEnumerable<short> shortList => shortList.Cast<int>().Sum(value => (int?)lambda.EvaluateAsync(value).GetAwaiter().GetResult()),
			IEnumerable<int> intList => intList.Sum(value => (int?)lambda.EvaluateAsync(value).GetAwaiter().GetResult()),
			IEnumerable<long> longList => longList.Sum(value => (long?)lambda.EvaluateAsync(value).GetAwaiter().GetResult()),
			IEnumerable<float> floatList => floatList.Sum(value => (float?)lambda.EvaluateAsync(value).GetAwaiter().GetResult()),
			IEnumerable<double> doubleList => doubleList.Sum(value => (double?)lambda.EvaluateAsync(value).GetAwaiter().GetResult()),
			IEnumerable<decimal> decimalList => decimalList.Sum(value => (decimal?)lambda.EvaluateAsync(value).GetAwaiter().GetResult()),
			_ => throw new FormatException($"First {ExtensionFunction.Sum} parameter must be an IEnumerable of a numeric type.")
		};
	}

	private static double GetSum(IEnumerable<object?> objectList)
	{
		double sum = 0;
		foreach (var item in objectList)
		{
			sum += item switch
			{
				byte byteValue => byteValue,
				short shortValue => shortValue,
				int intValue => intValue,
				long longValue => longValue,
				float floatValue => floatValue,
				double doubleValue => doubleValue,
				decimal decimalValue => (double)decimalValue,
				JValue jValue => jValue.Type switch
				{
					JTokenType.Float => jValue.Value<float>(),
					JTokenType.Integer => jValue.Value<int>(),
					_ => throw new FormatException($"Found unsupported JToken type '{jValue.Type}' when completing sum.")
				},
				null => 0,
				_ => throw new FormatException($"Found unsupported type '{item?.GetType().Name}' when completing sum.")
			};
		}

		return sum;
	}
}