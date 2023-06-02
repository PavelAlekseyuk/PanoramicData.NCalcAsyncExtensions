namespace NCalcAsyncExtensions.Test;

public class StoreAndRetrieveTests
{
	[Theory]
	[InlineData(null)]
	[InlineData(1)]
	[InlineData("a")]
	[InlineData(1.0)]
	public async Task Convert_Succeeds(object? value)
	{
		var insertedString =
			value is string ? $"'{value}'"
			: value is null ? "null"
			: value;
		var expression = $"convert(store('x', {insertedString}), retrieve('x'))";
		(await new ExtendedExpression(expression).EvaluateAsync())
			.Should()
			.Be(value);
	}

	[Theory]
	[InlineData("")]
	[InlineData("1")]
	[InlineData("1, 1, 1")]
	[InlineData("1, 1, 1, 1")]
	public async Task Store_IncorrectParameterCount_Throws(string parameters) =>
		await new ExtendedExpression($"store({parameters})")
		.Invoking(e => e.EvaluateAsync())
		.Should()
		.ThrowAsync<FormatException>()
		.WithMessage($"{ExtensionFunction.Store}() requires two parameters.");

	[Theory]
	[InlineData("")]
	[InlineData("1, 1")]
	[InlineData("1, 1, 1")]
	[InlineData("1, 1, 1, 1")]
	public async Task Retrieve_IncorrectParameterCount_Throws(string parameters) =>
		await new ExtendedExpression($"retrieve({parameters})")
		.Invoking(e => e.EvaluateAsync())
		.Should()
		.ThrowAsync<FormatException>()
		.WithMessage($"{ExtensionFunction.Retrieve}() requires one string parameter.");
}
