﻿using System.Collections.Generic;

namespace NCalcAsyncExtensions.Test;

public class SkipTests
{
	[Fact]
	public async Task List_OfInts_ReturnsExpectedType()
	{
		var expression = new ExtendedExpression($"skip(list(1, 2, 3), 1)");
		var result = await expression.EvaluateAsync();
		result.Should().BeOfType<List<object?>>();
	}

	[Fact]
	public async Task Array_OfInts_ReturnsExpectedType()
	{
		var expression = new ExtendedExpression($"skip(theArray, 1)");
		expression.Parameters["theArray"] = new int[] { 1, 2, 3 };
		var result = await expression.EvaluateAsync();
		result.Should().BeOfType<List<object?>>();
		result.Should().BeEquivalentTo(new List<object> { 2, 3 }, options => options.WithStrictOrdering());
	}

	[Fact]
	public async Task List_OfInts_ReturnsExpected()
	{
		var expression = new ExtendedExpression($"skip(list(1, 2, 3), 1)");
		(await expression.EvaluateAsync()).Should().BeEquivalentTo(new List<object> { 2, 3 }, options => options.WithStrictOrdering());
	}

	[Fact]
	public async Task SkippingTooMany_ReturnsExpected()
	{
		var expression = new ExtendedExpression($"skip(list(1, 2, 3), 10)");
		(await expression.EvaluateAsync()).Should().BeEquivalentTo(new List<object>(), options => options.WithStrictOrdering());
	}
}
