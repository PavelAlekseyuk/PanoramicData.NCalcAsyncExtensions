namespace NCalcAsyncExtensions.Test;

public class IsNullOrWhiteSpaceTests
{
	[Theory]
	[InlineData("'a'", false)]
	[InlineData("' '", true)]
	[InlineData("null", true)]
	[InlineData("''", true)]
	public async Task IsNullOrWhiteSpace_Succeeds(string parameter, bool expectedValue) 
		=> (await new ExtendedExpression($"isNullOrWhiteSpace({parameter})").EvaluateAsync()).Should().Be(expectedValue);

	[Theory]
	[InlineData("'a', 'a'")]
	[InlineData("")]
	[InlineData("'a', null, null")]
	public async Task IsNullOrWhiteSpace_Fails(string parameter) =>
		await new ExtendedExpression($"isNullOrWhiteSpace({parameter})")
			.Invoking(x => x.EvaluateAsync())
			.Should()
			.ThrowAsync<FormatException>().WithMessage("isNullOrWhiteSpace() requires one parameter.");
}
public class IsNullOrEmptyTests
{
	[Theory]
	[InlineData("'a'", false)]
	[InlineData("' '", false)]
	[InlineData("null", true)]
	[InlineData("''", true)]
	public async Task IsNullOrWhiteSpace_Succeeds(string parameter, bool expectedValue)
		=> (await new ExtendedExpression($"isNullOrEmpty({parameter})").EvaluateAsync()).Should().Be(expectedValue);

	[Theory]
	[InlineData("'a', 'a'")]
	[InlineData("")]
	[InlineData("'a', null, null")]
	public async Task IsNullOrWhiteSpace_Fails(string parameter) =>
		await new ExtendedExpression($"isNullOrEmpty({parameter})")
			.Invoking(x => x.EvaluateAsync())
			.Should()
			.ThrowAsync<FormatException>().WithMessage("isNullOrEmpty() requires one parameter.");
}
