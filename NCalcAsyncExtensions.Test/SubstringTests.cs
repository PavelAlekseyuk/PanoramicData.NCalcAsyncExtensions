namespace NCalcAsyncExtensions.Test;

public class SubstringTests
{
	[Theory]
	[InlineData("substring('haystack', 3)", "stack")]
	[InlineData("substring('haystack', 0, 3)", "hay")]
	[InlineData("substring('haystack', 3, 100)", "stack")]
	[InlineData("substring('haystack', 0, 100)", "haystack")]
	[InlineData("substring('haystack', 0, 0)", "")]
	public async Task Substring_HelpExamples_Succeed(string expressionText, string expected)
	{
		var expression = new ExtendedExpression(expressionText);
		Assert.Equal(expected, await expression.EvaluateAsync() as string);
	}

	[Fact]
	public async Task Substring_TwoParameters_Succeeds()
	{
		var expression = new ExtendedExpression("substring('ABC', 1)");
		Assert.Equal("BC", await expression.EvaluateAsync() as string);
	}

	[Fact]
	public async Task Substring_ThreeParameters_Succeeds()
	{
		var expression = new ExtendedExpression("substring('ABC', 1, 1)");
		Assert.Equal("B", await expression.EvaluateAsync() as string);
	}

	[Fact]
	public async Task Substring_ThreeParametersTruncate_Succeeds()
	{
		var expression = new ExtendedExpression("substring('ABC', 0, 6)");
		Assert.Equal("ABC", await expression.EvaluateAsync() as string);
	}

	[Fact]
	public async Task Substring_ThreeParametersTruncateMidString_Succeeds()
	{
		var expression = new ExtendedExpression("substring('ABC', 1, 6)");
		Assert.Equal("BC", await expression.EvaluateAsync() as string);
	}
}
