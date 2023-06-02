namespace NCalcAsyncExtensions.Test;

public class TypeOfTests
{
	[Theory]
	[InlineData("String", "'text'")]
	[InlineData("Int32", "1")]
	[InlineData("Double", "1.1")]
	[InlineData(null, "null")]
	public async Task TypeOf_ReturnsExpected(string? expected, string input)
	{
		var expression = new ExtendedExpression($"typeOf({input})");
		Assert.Equal(expected, await expression.EvaluateAsync());
	}
}
