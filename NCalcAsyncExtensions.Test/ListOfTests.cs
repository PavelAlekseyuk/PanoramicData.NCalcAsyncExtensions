using System.Collections.Generic;

namespace NCalcAsyncExtensions.Test;

public class ListOfTests
{
	[Fact]
	public async Task List_OfInts_ReturnsExpectedType()
	{
		var expression = new ExtendedExpression($"listOf('int', 1, 2, 3)");
		(await expression.EvaluateAsync()).Should().BeOfType<List<int>>();
	}

	[Fact]
	public async Task List_OfInts_ReturnsExpected()
	{
		var expression = new ExtendedExpression($"listOf('int', 1, 2, 3)");
		(await expression.EvaluateAsync()).Should().BeEquivalentTo(new List<int> { 1, 2, 3 }, options => options.WithStrictOrdering());
	}

	[Fact]
	public async Task List_OfLongs_IsForgivingToInts()
	{
		var expression = new ExtendedExpression($"listOf('long', 1, 2, 3)");
		(await expression.EvaluateAsync()).Should().BeEquivalentTo(new List<long> { 1, 2, 3 }, options => options.WithStrictOrdering());
	}

	[Fact]
	public async Task List_OfInts_IsForgivingToDoubles()
	{
		var expression = new ExtendedExpression($"listOf('int', 1.0, 2.0, 3.1)");
		(await expression.EvaluateAsync()).Should().BeEquivalentTo(new List<int> { 1, 2, 3 }, options => options.WithStrictOrdering());
	}

	[Fact]
	public async Task List_OfDoubles_IsForgivingToInts()
	{
		var expression = new ExtendedExpression($"listOf('double', 1, 2, 3)");
		(await expression.EvaluateAsync()).Should().BeEquivalentTo(new List<double> { 1, 2, 3 }, options => options.WithStrictOrdering());
	}

	[Fact]
	public async Task List_OfExpressions_ReturnsExpected()
	{
		var expression = new ExtendedExpression($"listOf('int', 2 - 1, 2 + 0, 5 - 2)");
		(await expression.EvaluateAsync()).Should().BeEquivalentTo(new List<int> { 1, 2, 3 }, options => options.WithStrictOrdering());
	}

	[Fact]
	public async Task List_OfStrings_ReturnsExpected()
	{
		var expression = new ExtendedExpression($"listOf('string', '1', '2', '3')");
		(await expression.EvaluateAsync()).Should().BeEquivalentTo(new List<string> { "1", "2", "3" }, options => options.WithStrictOrdering());
	}

	[Fact]
	public async Task List_WhichIsEmpty_ReturnsExpected()
	{
		var expression = new ExtendedExpression($"listOf('string')");
		(await expression.EvaluateAsync()).Should().BeEquivalentTo(new List<string>(), options => options.WithStrictOrdering());
	}

	[Fact]
	public async Task List_OfMixedTypes_ReturnsExpected()
	{
		var expression = new ExtendedExpression($"listOf('object', null, 1-1, '1')");
		(await expression.EvaluateAsync()).Should().BeEquivalentTo(new List<object> { null!, 0, "1" }, options => options.WithStrictOrdering());
	}
}
