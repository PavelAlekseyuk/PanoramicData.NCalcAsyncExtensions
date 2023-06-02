namespace NCalcAsyncExtensions.Test;

public class ItemAtIndexTests : NCalcTest
{
	[Theory]
	[InlineData("itemAtIndex()")]
	[InlineData("itemAtIndex('a b c')")]
	[InlineData("itemAtIndex('a b c', null)")]
	[InlineData("itemAtIndex('a b c', 'xxx')")]
	public async Task ItemAtIndex_InsufficientParameters_ThrowsException(string expression)
		=> await Assert.ThrowsAsync<FormatException>(()=>
		{
			var e = new ExtendedExpression(expression);
			return e.EvaluateAsync();
		});

	[Theory]
	[InlineData("itemAtIndex(split('a b c', ' '), 1)", "b")]
	public async Task ItemAtIndex_ReturnsExpected(string expression, object? expectedOutput)
		=> Assert.Equal(expectedOutput, await new ExtendedExpression(expression).EvaluateAsync());
}