namespace NCalcAsyncExtensions.Test;

public class DateTimeAsEpochMsTests : NCalcTest
{
	[Fact]
	public async Task AllParameters_Succeeds()
	{
		var result = await TestAsync("dateTimeAsEpochMs('20190702T000000', 'yyyyMMddTHHmmssK')");
		const long desiredDateTimeEpochMs = 1562025600000;
		Assert.Equal(desiredDateTimeEpochMs, result);
	}

	[Fact]
	public async Task UsingSquareBrackets_Succeeds()
	{
		var expression = new ExtendedExpression("1 > dateTimeAsEpochMs([connectMagic.systemItem.sys_updated_on], 'yyyy-MM-dd HH:mm:ss')");
		expression.Parameters.Add("connectMagic.systemItem.sys_updated_on", "2018-01-01 01:01:01");
		var result = await expression.EvaluateAsync();
		Assert.Equal(false, result);
	}
}
