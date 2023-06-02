namespace NCalcAsyncExtensions.Test;

public class InterClassTests
{
	[Fact]
	public async Task AddingAnIntStringToAnInt_YieldsInt()
	{
		var expression = new ExtendedExpression("0 + '1'");
		(await expression.EvaluateAsync()).Should().BeOfType<decimal>();
		(await expression.EvaluateAsync()).Should().Be(1);
		(await expression.EvaluateAsync()).Should().NotBe("1");
	}

	[Fact]
	public async Task AddingAnIntToAnIntString_YieldsString()
	{
		var expression = new ExtendedExpression("'1' + 0");
		(await expression.EvaluateAsync()).Should().BeOfType<string>();
		(await expression.EvaluateAsync()).Should().Be("10");
		(await expression.EvaluateAsync()).Should().NotBe(10);
	}
}
