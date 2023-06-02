namespace NCalcAsyncExtensions.Test;

public class LengthTests
{
	[Fact]
	public async Task Length_OfString_ReturnsExpectedResult()
	{
		var expression = new ExtendedExpression($"length('a piece of string')");
		var result = await expression.EvaluateAsync();
		result.Should().Be(17);
	}

	[Fact]
	public async Task Length_OfList_ReturnsExpectedResult()
	{
		var expression = new ExtendedExpression($"length(split('a piece of string', ' '))");
		var result = await expression.EvaluateAsync();
		result.Should().Be(4);
	}
}
