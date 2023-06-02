using System.Collections.Generic;
using System.Linq;

namespace NCalcAsyncExtensions.Test;

public class CountTests
{
	private readonly List<string> _stringList = new() { "a", "b", "c" };

	[Fact]
	public async Task Count_OfList_ReturnsExpectedResult()
	{
		var expression = new ExtendedExpression($"count(split('a piece of string', ' '))");
		var result = await expression.EvaluateAsync();
		result.Should().Be(4);
	}

	[Fact]
	public async Task Count_OfListOfString_ReturnsExpectedResult()
	{
		var expression = new ExtendedExpression($"count(x)");
		expression.Parameters.Add("x", _stringList);
		var result = await expression.EvaluateAsync();
		result.Should().Be(_stringList.Count);
	}

	[Fact]
	public async Task Count_WithLambda_ReturnsExpectedResult()
	{
		var expression = new ExtendedExpression($"count(list(1,2,3), 'n', 'n < 3')");
		var result = await expression.EvaluateAsync();
		result.Should().Be(2);
	}

	[Fact]
	public async Task Count_OfEnumerableOfString_ReturnsExpectedResult()
	{
		var expression = new ExtendedExpression($"count(x)");
		expression.Parameters.Add("x", _stringList.AsEnumerable());
		var result = await expression.EvaluateAsync();
		result.Should().Be(_stringList.Count);
	}

	[Fact]
	public async Task Count_OfReadOnlyOfString_ReturnsExpectedResult()
	{
		var expression = new ExtendedExpression($"count(x)");
		expression.Parameters.Add("x", _stringList.AsReadOnly());
		var result = await expression.EvaluateAsync();
		result.Should().Be(_stringList.Count);
	}

	[Fact]
	public async Task Count_JArray_ReturnsExpectedResult()
	{
		var expression = new ExtendedExpression($"count(x)");
		expression.Parameters.Add("x", JArray.FromObject(_stringList));
		var result = await expression.EvaluateAsync();
		result.Should().Be(_stringList.Count);
	}
}
