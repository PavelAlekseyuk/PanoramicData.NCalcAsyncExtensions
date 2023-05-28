using System.Collections.Generic;

namespace PanoramicData.NCalcAsyncExtensions.Test;

public class ConcatTests
{
	[Fact]
	public async Task OneListsOfInts_Succeeds()
	{
		var expression = new ExtendedExpression($"concat(list(1, 2, 3))");
		var result = await expression.EvaluateAsync();
		result.Should().BeOfType<List<object?>>();
		result.Should().BeEquivalentTo(new List<object> { 1, 2, 3 }, options => options.WithStrictOrdering());
	}

	[Fact]
	public async Task TwoListsOfInts_Succeeds()
	{
		var expression = new ExtendedExpression($"concat(list(1), list(2, 3))");
		var result = await expression.EvaluateAsync();
		result.Should().BeOfType<List<object?>>();
		result.Should().BeEquivalentTo(new List<object> { 1, 2, 3 }, options => options.WithStrictOrdering());
	}

	[Fact]
	public async Task ThreeListsOfInts_Succeeds()
	{
		var expression = new ExtendedExpression($"concat(list(1), list(2), list(3))");
		var result = await expression.EvaluateAsync();
		result.Should().BeOfType<List<object?>>();
		result.Should().BeEquivalentTo(new List<object> { 1, 2, 3 }, options => options.WithStrictOrdering());
	}

	[Fact]
	public async Task ListOfIntsAddingOneObject_Succeeds()
	{
		var expression = new ExtendedExpression($"concat(list(1, 2), 3)");
		var result = await expression.EvaluateAsync();
		result.Should().BeOfType<List<object?>>();
		result.Should().BeEquivalentTo(new List<object> { 1, 2, 3 }, options => options.WithStrictOrdering());
	}

	[Fact]
	public async Task OneObjectAddingListOfInts_Succeeds()
	{
		var expression = new ExtendedExpression($"concat(1, list(2, 3))");
		var result = await expression.EvaluateAsync();
		result.Should().BeOfType<List<object?>>();
		result.Should().BeEquivalentTo(new List<object> { 1, 2, 3 }, options => options.WithStrictOrdering());
	}
}
