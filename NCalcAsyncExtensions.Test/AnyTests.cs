namespace NCalcAsyncExtensions.Test;

public class AnyTests : NCalcTest
{
	[Theory]
	[InlineData("1, 2, 3", true)]
	[InlineData("7, 8, 9", false)]
	public async Task Any_LessThanFive_Succeeds(string stringList, bool anyLessThanFive)
	{
		var expression = new ExtendedExpression($"any(list({stringList}), 'n', 'n < 5')");
		var result = await expression.EvaluateAsync();
		result.Should().Be(anyLessThanFive);
	}
}
