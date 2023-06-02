using NCalcAsyncExtensions.Helpers;
using System.Collections.Generic;

namespace NCalcAsyncExtensions;

public class AsyncLambda
{
	private readonly string predicate;
	private readonly string nCalcString;
	private readonly Dictionary<string, object?> parameters;

	public AsyncLambda(string predicate, string nCalcString, Dictionary<string, object?> parameters)
	{
		this.predicate = predicate;
		this.nCalcString = nCalcString;
		this.parameters = parameters;
	}

	public async Task<object?> EvaluateAsync<T>(T value)
	{
		parameters.Remove(predicate);
		parameters.Add(predicate, value!);
		var ncalc = new ExtendedExpression(nCalcString) { Parameters = parameters };

		return await ncalc.EvaluateSafelyAsync();
	}
}