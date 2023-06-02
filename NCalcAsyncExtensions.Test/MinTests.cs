using System.Linq;

namespace NCalcAsyncExtensions.Test;

public class MinTests
{
	[Theory]
	[InlineData("1, 2, 3", 1)]
	[InlineData("3, 2, 1", 1)]
	[InlineData("1, 3, 2", 1)]
	[InlineData("1, 1, 1", 1)]
	[InlineData("1, 1, 2", 1)]
	[InlineData("1, null, 2", 1)]
	[InlineData("1.1, null, 2", 1.1)]
	[InlineData("null, null, null", null)]

	public async Task Min_OfNumbers_ReturnsExpectedValue(string values, object expectedOutput)
	{
		var expression = new ExtendedExpression($"min(listOf('double?', {values}), 'x', 'x')");
		(await expression.EvaluateAsync()).Should().BeEquivalentTo(expectedOutput);
	}

	[Theory]
	[InlineData("'1', '2', '3'", "1")]
	[InlineData("'3', '2', '1'", "1")]
	[InlineData("'1', '3', null", "1")]
	[InlineData("'abc', 'raf', 'bbc'", "abc")]
	[InlineData("'abc', 'ABC', null", "abc")]

	public async Task Min_OfStrings_ReturnsExpectedValue(string values, string expectedOutput)
	{
		var expression = new ExtendedExpression($"min(list({values}))");
		(await expression.EvaluateAsync()).Should().BeEquivalentTo(expectedOutput);
	}

	[Theory]
	[InlineData("1,2,3", "1")]
	[InlineData("3,2,1", "1")]
	[InlineData("1,3,null", "1")]
	[InlineData("abc,raf,bbc", "abc")]
	[InlineData("abc,ABC,null", "abc")]

	public async Task Min_OfStringsAsVariable_ReturnsExpectedValue(string values, string expectedOutput)
	{
		var expression = new ExtendedExpression($"min(valuesList)");
		expression.Parameters["valuesList"] = values.Split(',').Select(x => x == "null" ? null : x).ToList();
		(await expression.EvaluateAsync()).Should().BeEquivalentTo(expectedOutput);
	}

	[Fact]

	public async Task Min_OfNull_ReturnsExpectedValue()
	{
		var expression = new ExtendedExpression($"min(null)");
		(await expression.EvaluateAsync()).Should().BeNull();
	}

	[Fact]
	public async Task Min_OfEmptyList_ReturnsNull()
	{
		var expression = new ExtendedExpression($"min(list())");
		(await expression.EvaluateAsync()).Should().BeNull();
	}

	[Fact]
	public async Task Max_UsingLambdaForInt_ReturnsExpected()
	{
		var expression = new ExtendedExpression($"min(listOf('int', 1, 2, 3), 'x', 'x + 1')");
		(await expression.EvaluateAsync()).Should().Be(2);
	}

	[Fact]
	public async Task Max_UsingLambdaForString_ReturnsExpected()
	{
		var expression = new ExtendedExpression("min(listOf('string', '1', '2', '3'), 'x', 'x + x')");
		(await expression.EvaluateAsync()).Should().Be("11");
	}
}