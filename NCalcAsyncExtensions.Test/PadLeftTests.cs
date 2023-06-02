namespace NCalcAsyncExtensions.Test;

public class PadLeftTests
{
	[Fact]
	public async Task Padleft_EmptyString_SucceedsWithPaddedString()
	{
		var expression = new ExtendedExpression("padLeft('', 1, '0')");
		Assert.Equal("0", await expression.EvaluateAsync() as string);
	}

	[Fact]
	public async Task Padleft_DesiredLengthGreaterThanInput_SucceedsWithPaddedString()
	{
		var expression = new ExtendedExpression("padLeft('12', 5, '0')");
		Assert.Equal("00012", await expression.EvaluateAsync() as string);
	}

	[Fact]
	public async Task Padleft_DesiredLengthEqualToInput_SucceedsWithOriginalString()
	{
		var expression = new ExtendedExpression("padLeft('12345', 5, '0')");
		Assert.Equal("12345", await expression.EvaluateAsync() as string);
	}

	[Fact]
	public async Task Padleft_DesiredLengthLessThanInput_SucceedsWithOriginalString()
	{
		var expression = new ExtendedExpression("padLeft('12345', 2, '0')");
		Assert.Equal("12345", await expression.EvaluateAsync() as string);
	}

	[Fact]
	public async Task Padleft_DesiredStringLengthTooLow_FailsAsExpected()
	{
		var expression = new ExtendedExpression("padLeft('12345', 0, '0')");
		var exception = await Assert.ThrowsAsync<NCalcExtensionsException>(expression.EvaluateAsync);
		exception.Message.Should().Be("padLeft() requires a DesiredStringLength for parameter 2 that is >= 1.");
	}

	[Fact]
	public async Task Padleft_PaddingStringTooLong_FailsAsExpected()
	{
		var expression = new ExtendedExpression("padLeft('12345', 5, '00')");
		var exception = await Assert.ThrowsAsync<NCalcExtensionsException>(expression.EvaluateAsync);
		exception.Message.Should().Be("padLeft() requires a single character string for parameter 3.");
	}

	[Fact]
	public async Task Padleft_PaddingStringEmpty_FailsAsExpected()
	{
		var expression = new ExtendedExpression("padLeft('12345', 5, '')");
		var exception = await Assert.ThrowsAsync<NCalcExtensionsException>(expression.EvaluateAsync);
		exception.Message.Should().Be("padLeft() requires a single character string for parameter 3.");
	}
}
