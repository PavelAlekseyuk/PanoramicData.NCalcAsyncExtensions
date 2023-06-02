namespace NCalcAsyncExtensions.Test;

public class JoinTests : NCalcTest
{

	[Fact]
	public async Task Join_Simple_Succeeds()
	{
		var expression = new ExtendedExpression("join(list('a', 'b', 'c'), ', ')");
		var result = await expression.EvaluateAsync();
		result.Should().BeOfType<string>();
		result.Should().Be("a, b, c");
	}

	[Fact]
	public async Task Join_Array_Succeeds()
	{
		var expression = new ExtendedExpression("join(array, ', ')");
		expression.Parameters["array"] = new[] { "a", "b", "c" };
		var result = await expression.EvaluateAsync();
		result.Should().BeOfType<string>();
		result.Should().Be("a, b, c");
	}
}
