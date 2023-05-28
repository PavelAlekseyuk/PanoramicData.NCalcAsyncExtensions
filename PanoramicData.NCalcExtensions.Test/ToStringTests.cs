namespace PanoramicData.NCalcAsyncExtensions.Test;

public class ToStringTests
{
	[Fact]
	public async Task ToString_IsNull_ReturnsNull()
	{
		var expression = new ExtendedExpression("toString(null)");
		(await expression.EvaluateAsync()).Should().BeNull();
	}

	[Fact]
	public async Task ToString_Int_Succeeds()
	{
		var expression = new ExtendedExpression("toString(1)");
		(await expression.EvaluateAsync()).Should().Be("1");
	}

	[Fact]
	public async Task ToString_Int_Formatted_Succeeds()
	{
		var expression = new ExtendedExpression("toString(1000, 'N2')");
		(await expression.EvaluateAsync()).Should().Be("1,000.00");
	}

	[Fact]
	public async Task ToString_DateTime_Formatted_Succeeds()
	{
		var expression = new ExtendedExpression("toString(TheDateTime, 'yyyy-MM-dd')");
		expression.Parameters.Add("TheDateTime", new DateTime(2020, 1, 1));
		(await expression.EvaluateAsync()).Should().Be("2020-01-01");
	}

	[Fact]
	public async Task ToString_DateTimeOffset_Formatted_Succeeds()
	{
		var expression = new ExtendedExpression("toString(TheDateTimeOffset, 'yyyy-MM-dd')");
		expression.Parameters.Add("TheDateTimeOffset", new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero));
		(await expression.EvaluateAsync()).Should().Be("2020-01-01");
	}
}
