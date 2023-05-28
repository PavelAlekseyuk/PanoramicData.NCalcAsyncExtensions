using PanoramicData.NCalcAsyncExtensions.Exceptions;

namespace PanoramicData.NCalcAsyncExtensions.Test;

public class TryTests
{
	[Fact]
	public async Task Try_ToFewParameters_Fails()
		=> await new ExtendedExpression("try()").Invoking(y => y.EvaluateAsync())
			.Should()
			.ThrowAsync<FormatException>()
			.WithMessage("try: At least 1 parameter required.");

	[Fact]
	public async Task Try_ToManyParameters_Fails()
	  => await new ExtendedExpression("try(throw('Woo'), 2, 3)").Invoking(y => y.EvaluateAsync())
		  .Should()
		  .ThrowAsync<FormatException>()
		  .WithMessage("try: No more than 2 parameters permitted.");

	[Theory]
	[InlineData("1", 1)]
	[InlineData("throw('Woo')", null)]
	[InlineData("throw('Woo'), 1", 1)]
	[InlineData("throw('Woo'), exception_message", "Woo")]
	[InlineData("throw('Woo'), exception_typeName", nameof(NCalcExtensionsException))]
	[InlineData("throw('Woo'), exception_type", typeof(NCalcExtensionsException))]
	[InlineData("throw('Woo'), exception_typeFullName", "PanoramicData.NCalcAsyncExtensions.Exceptions.NCalcExtensionsException")]
	public async Task Try_SimpleNoThrow_Succeeds(string parameters, object? expectedValue)
	{
		var result = await new ExtendedExpression($"try({parameters})").EvaluateAsync();
		result.Should().Be(expectedValue);
	}
}
