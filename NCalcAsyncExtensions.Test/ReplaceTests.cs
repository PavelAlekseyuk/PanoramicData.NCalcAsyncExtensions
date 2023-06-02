namespace NCalcAsyncExtensions.Test;

public class ReplaceTests
{
	[Fact]
	public async Task Replace_Example1_Succeeds()
	{
		var expression = new ExtendedExpression("replace('abcdefg', 'cde', 'CDE')");
		Assert.Equal("abCDEfg", await expression.EvaluateAsync() as string);
	}

	[Fact]
	public async Task Replace_Example2_Succeeds()
	{
		var expression = new ExtendedExpression("replace('abcdefg', 'cde', '')");
		Assert.Equal("abfg", await expression.EvaluateAsync() as string);
	}
}
