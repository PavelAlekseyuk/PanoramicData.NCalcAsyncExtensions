using System.Collections.Generic;
using System.Linq;

namespace PanoramicData.NCalcAsyncExtensions.Test;

public class SumTests
{
	private readonly List<int> _intList = new() { 1, 2, 3 };
	private readonly List<object?> _objectList = new() { 1f, 2d, 3, null };

	[Fact]
	public async Task Sum_WithLambda_ReturnsExpectedResult()
	{
		var expression = new ExtendedExpression($"sum(x, 'n', 'n * n')");
		expression.Parameters.Add("x", _intList);
		var result = await expression.EvaluateAsync();
		result.Should().Be(_intList.Sum(n => n * n));
	}

	[Fact]
	public async Task Sum_OfEnumerableOfInt_ReturnsExpectedResult()
	{
		var expression = new ExtendedExpression($"sum(x)");
		expression.Parameters.Add("x", _intList.AsEnumerable());
		var result = await expression.EvaluateAsync();
		result.Should().Be(_intList.Sum());
	}

	[Fact]
	public async Task Sum_OfListOfInt_ReturnsExpectedResult()
	{
		var expression = new ExtendedExpression($"sum(x)");
		expression.Parameters.Add("x", _intList);
		var result = await expression.EvaluateAsync();
		result.Should().Be(_intList.Sum());
	}

	[Fact]
	public async Task Sum_OfListOfObject_ReturnsExpectedResult()
	{
		var expression = new ExtendedExpression($"sum(x)");
		expression.Parameters.Add("x", _objectList);
		var result = await expression.EvaluateAsync();
		result.Should().Be(_intList.Sum());
	}
}
