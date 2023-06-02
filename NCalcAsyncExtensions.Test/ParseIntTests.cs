namespace NCalcAsyncExtensions.Test;

public class ParseIntTests
{
	[Fact]
	public async Task ParseInt_NoParameter_Throws()
	{
		var expression = new ExtendedExpression("parseInt()");
		await Assert.ThrowsAsync<FormatException>(expression.EvaluateAsync);
	}

	[Fact]
	public async Task ParseInt_TooManyParameters_Throws()
	{
		var expression = new ExtendedExpression("parseInt('1', '2')");
		await Assert.ThrowsAsync<FormatException>(expression.EvaluateAsync);
	}

	[Fact]
	public async Task ParseInt_NotAString_Throws()
	{
		var expression = new ExtendedExpression("parseInt(1)");
		await Assert.ThrowsAsync<FormatException>(expression.EvaluateAsync);
	}

	[Fact]
	public async Task ParseInt_Valid_Succeeds()
	{
		var expression = new ExtendedExpression("parseInt('1')");
		var result = await expression.EvaluateAsync();
		result.Should().BeOfType<int>();
		result.Should().Be(1);
	}
}
