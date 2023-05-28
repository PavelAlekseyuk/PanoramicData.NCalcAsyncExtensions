namespace PanoramicData.NCalcAsyncExtensions.Test;

public class ParameterTests
{
	[Fact]
	public async Task SquareBracketParameter_Succeeds()
	{
		var expression = new ExtendedExpression("[a.b]");
		expression.Parameters["a.b"] = "AAAA";
		Assert.Equal("AAAA", await expression.EvaluateAsync() as string);
	}
}
