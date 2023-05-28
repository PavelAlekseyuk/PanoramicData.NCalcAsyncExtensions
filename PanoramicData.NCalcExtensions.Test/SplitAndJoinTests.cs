namespace PanoramicData.NCalcAsyncExtensions.Test;

public class SplitAndJoinTests : NCalcTest
{
	[Theory]
	[InlineData("split()")]
	[InlineData("split('a b c')")]
	public async Task Switch_InsufficientParameters_ThrowsException(string expression)
		=> await Assert.ThrowsAsync<FormatException>(() => new ExtendedExpression(expression).EvaluateAsync());

	[Theory]
	[InlineData("join()")]
	[InlineData("join(1)")]
	public async Task Join_InsufficientParameters_ThrowsException(string expression) 
		=> await Assert.ThrowsAsync<FormatException>(() => new ExtendedExpression(expression).EvaluateAsync());

	[Theory]
	[InlineData("join(split('a b c', ' '), ',')", "a,b,c")]
	[InlineData("join(split('a b c', ' '), ', ')", "a, b, c")]
	[InlineData("join(list('a', 'b', 'c'), ', ')", "a, b, c")]
	public async Task Switch_ReturnsExpected(string expression, object? expectedOutput)
		=> Assert.Equal(expectedOutput, await new ExtendedExpression(expression).EvaluateAsync());
}
