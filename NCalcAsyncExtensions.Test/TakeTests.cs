using System.Collections.Generic;

namespace NCalcAsyncExtensions.Test;

public class TakeTests
{
	[Fact]
	public async Task List_OfInts_ReturnsExpectedType()
	{
		var expression = new ExtendedExpression($"take(list(1, 2, 3), 1)");
		(await expression.EvaluateAsync()).Should().BeOfType<List<object?>>();
	}

	[Fact]
	public async Task Array_OfInts_ReturnsExpectedType()
	{
		var expression = new ExtendedExpression($"take(theArray, 1)");
		expression.Parameters["theArray"] = new int[] { 1, 2, 3 };
		var result = await expression.EvaluateAsync();
		result.Should().BeOfType<List<object?>>();
		result.Should().BeEquivalentTo(new[] { 1 });
	}

	[Fact]
	public async Task List_OfInts_ReturnsExpected()
	{
		var expression = new ExtendedExpression($"take(list('a', 2, 3), 1)");
		(await expression.EvaluateAsync()).Should().BeEquivalentTo(new List<object> { "a" }, options => options.WithStrictOrdering());
	}

	[Fact]
	public async Task TakingTooMany_ReturnsExpected()
	{
		var expression = new ExtendedExpression($"take(list(1, 2, 3), 10)");
		(await expression.EvaluateAsync()).Should().BeEquivalentTo(new List<object> { 1, 2, 3 }, options => options.WithStrictOrdering());
	}
}
