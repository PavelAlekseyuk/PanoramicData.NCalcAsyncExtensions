using System.Runtime.CompilerServices;

namespace NCalcAsyncExtensions.Helpers;

public static class ExpressionExtensions
{
	public static ConfiguredTaskAwaitable<object> EvaluateSafelyAsync(this Expression expression) => expression.EvaluateAsync().ConfigureAwait(false);
}