namespace PanoramicData.NCalcAsyncExtensions.Test;

public class GetPropertyTests
{
	[Fact]
	public async Task GetProperty()
	{
		var year = 2019;
		var expression = new ExtendedExpression($"getProperty(toDateTime('{year}-01-01', 'yyyy-MM-dd'), 'Year')");
		var result = await expression.EvaluateAsync();
		result.Should().BeOfType<int>();
		result.Should().Be(year);
	}

	[Fact]
	public async Task GetProperty_FromJObject()
	{
		var expression = new ExtendedExpression($"getProperty(parse('jObject', '{{ \"A\": 1, \"B\": 2 }}'), 'B')");
		var result = await expression.EvaluateAsync();
		result.Should().BeOfType<int>();
		result.Should().Be(2);
	}
}
