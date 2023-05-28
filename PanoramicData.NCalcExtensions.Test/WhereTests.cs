namespace PanoramicData.NCalcAsyncExtensions.Test;

public class WhereTests : NCalcTest
{
	[Theory]
	[InlineData("n == 2", 1)]
	[InlineData("n % 2 == 0", 2)]
	public async Task Where_Succeeds(string expression, int expectedCount) =>
		(await new ExtendedExpression($"length(where(list(1, 2, 3, 4, 5), 'n', '{expression}'))").EvaluateAsync())
		.Should()
		.Be(expectedCount);
}
