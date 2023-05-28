namespace PanoramicData.NCalcAsyncExtensions.Test;

public class ConvertTests
{
	[Theory]
	[InlineData("null", "null", null)]
	[InlineData("1", "null", null)]
	[InlineData("null", "1", 1)]
	[InlineData("null", "value", null)]
	[InlineData("1", "value", 1)]
	[InlineData("1 + 1", "value", 2)]
	[InlineData("store('x', 1)", "retrieve('x')", 1)]
	public async Task Convert_Succeeds(string firstParameter, string secondParameter, object? expectedResult)
	{
		(await new ExtendedExpression($"convert({firstParameter}, {secondParameter})").EvaluateAsync())
			.Should()
			.Be(expectedResult);
	}

	[Theory]
	[InlineData("")]
	[InlineData("1")]
	[InlineData("1, 1, 1")]
	[InlineData("1, 1, 1, 1")]
	public async Task IncorrectParameterCount_Throws(string parameters)
	{
		var expression = new ExtendedExpression($"convert({parameters})");
		await expression.Invoking(e => e.EvaluateAsync())
			.Should()
			.ThrowAsync<FormatException>()
			.WithMessage($"{ExtensionFunction.Convert}() requires two parameters.");
	}
}
