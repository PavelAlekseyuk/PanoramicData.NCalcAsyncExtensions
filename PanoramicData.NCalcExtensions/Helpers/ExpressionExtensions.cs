using System.Runtime.CompilerServices;

namespace PanoramicData.NCalcAsyncExtensions.Helpers;

public static class ExpressionExtensions
{
	public static ConfiguredTaskAwaitable<object> EvaluateSafelyAsync(this Expression expression) => expression.EvaluateAsync().ConfigureAwait(false);
}