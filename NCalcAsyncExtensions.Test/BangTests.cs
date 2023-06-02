namespace NCalcAsyncExtensions.Test;

public class BangTests : NCalcTest
{
	[Fact]
	public async Task Bang_Succeeds()
	{
		const string expression = "!(1 == 2)";
		var e = new ExtendedExpression(expression);
		var result = await e.EvaluateAsync();
		result.Should().Be(true);
	}
}
