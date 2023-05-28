using System.Collections.Generic;

namespace PanoramicData.NCalcAsyncExtensions.Test;

public class ListTests
{
	[Fact]
	public async Task List_OfInts_ReturnsExpectedType()
	{
		var expression = new ExtendedExpression($"list(1, 2, 3)");
		(await expression.EvaluateAsync()).Should().BeOfType<List<object?>>();
	}

	[Fact]
	public async Task List_OfInts_ReturnsExpected()
	{
		var expression = new ExtendedExpression($"list(1, 2, 3)");
		(await expression.EvaluateAsync()).Should().BeEquivalentTo(new List<object> { 1, 2, 3 }, options => options.WithStrictOrdering());
	}

	[Fact]
	public async Task List_OfExpressions_ReturnsExpected()
	{
		var expression = new ExtendedExpression($"list(2 - 1, 2 + 0, 5 - 2)");
		(await expression.EvaluateAsync()).Should().BeEquivalentTo(new List<object> { 1, 2, 3 }, options => options.WithStrictOrdering());
	}

	[Fact]
	public async Task List_OfStrings_ReturnsExpected()
	{
		var expression = new ExtendedExpression($"list('1', '2', '3')");
		(await expression.EvaluateAsync()).Should().BeEquivalentTo(new List<object> { "1", "2", "3" }, options => options.WithStrictOrdering());
	}

	[Fact]
	public async Task List_WhichIsEmpty_ReturnsExpected()
	{
		var expression = new ExtendedExpression($"list()");
		(await expression.EvaluateAsync()).Should().BeEquivalentTo(new List<object>(), options => options.WithStrictOrdering());
	}

	[Fact]
	public async Task List_OfMixedTypes_ReturnsExpected()
	{
		var expression = new ExtendedExpression($"list(null, 1-1, '1')");
		(await expression.EvaluateAsync()).Should().BeEquivalentTo(new List<object> { null!, 0, "1" }, options => options.WithStrictOrdering());
	}
}
