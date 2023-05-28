namespace PanoramicData.NCalcAsyncExtensions.Test;

public class ChangeTimeZonesTests : NCalcTest
{
	[Fact]
	public async Task UtcToEst_Succeeds()
	{
		const string parameterName = "theDateTimeUtc";
		var expression = new ExtendedExpression($"changeTimeZone({parameterName}, 'UTC', 'Eastern Standard Time')");
		expression.Parameters[parameterName] = new DateTime(2020, 03, 13, 16, 00, 00);
		var result = await expression.EvaluateAsync();
		Assert.Equal(new DateTime(2020, 03, 13, 12, 00, 00), result);
	}

	[Fact]
	public async Task EstToUtc_Succeeds()
	{
		const string parameterName = "theDateTimeUtc";
		var expression = new ExtendedExpression($"changeTimeZone({parameterName}, 'Eastern Standard Time', 'UTC')");
		expression.Parameters[parameterName] = new DateTime(2020, 03, 13, 12, 00, 00);
		var result = await expression.EvaluateAsync();
		Assert.Equal(new DateTime(2020, 03, 13, 16, 00, 00), result);
	}
}
