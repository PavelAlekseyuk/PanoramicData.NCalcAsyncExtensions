namespace PanoramicData.NCalcAsyncExtensions.Test;

public abstract class NCalcTest
{
	protected static Task<object> TestAsync(string expressionText)
	{
		var expression = new ExtendedExpression(expressionText);
		return expression.EvaluateAsync();
	}
}
