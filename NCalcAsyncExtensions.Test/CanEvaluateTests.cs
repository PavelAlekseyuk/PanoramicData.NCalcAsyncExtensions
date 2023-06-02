namespace NCalcAsyncExtensions.Test;

public class CanEvaluateTests
{
	[Fact]
	public async Task CanEvaluate_Example1_Succeeds()
	{
		var expression = new ExtendedExpression("canEvaluate(nonExistent)");
		(await expression.EvaluateAsync() as bool?).Should().BeFalse();
	}

	[Fact]
	public async Task CanEvaluate_Example2_Succeeds()
	{
		var expression = new ExtendedExpression("canEvaluate(1)");
		(await expression.EvaluateAsync() as bool?).Should().BeTrue();
	}
}
