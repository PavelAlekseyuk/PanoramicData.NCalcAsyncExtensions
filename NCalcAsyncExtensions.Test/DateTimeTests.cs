namespace NCalcAsyncExtensions.Test;

public class DateTimeTests : NCalcTest
{
	[Fact]
	public async Task AllParameters_Succeeds()
	{
		const string format = "yyyy-MM-dd HH:mm";
		var result = await TestAsync($"dateTime('UTC', '{format}', -90, 0, 0, 0)");
		var desiredDateTime = DateTime.UtcNow.AddDays(-90);
		Assert.Equal(desiredDateTime.ToString(format, CultureInfo.InvariantCulture), result);
	}

	[Fact]
	public async Task Thing_Succeeds()
	{
		var result = await TestAsync("timespan(format(toDateTime('2020-01-01T00:00:00.000', 'yyyy-MM-ddTHH:mm:ss.FFF', 'Eastern Standard Time'), 'yyyy-MM-dd HH:mm:ss', 'UTC'), dateTime('UTC', 'yyyy-MM-dd HH:mm:ss'), 'seconds')");
		var resultAsTimeSpan = result as double?;
		Assert.True(resultAsTimeSpan.HasValue);
	}

	[Fact]
	public async Task Thing2_Succeeds()
	{
		//                 timespan(format(toDateTime(incident_CreateDate      , 'yyyy-MM-ddTHH:mm:ss.FFF', 'Eastern Standard Time'), 'yyyy-MM-dd HH:mm:ss', 'UTC'), dateTime('UTC', 'yyyy-MM-dd HH:mm:ss'), 'seconds') <= 600
		var result = await TestAsync("timespan(format(toDateTime('2020-01-01T00:00:00.000', 'yyyy-MM-ddTHH:mm:ss.FFF', 'Eastern Standard Time'), 'yyyy-MM-dd HH:mm:ss', 'UTC'), dateTime('UTC', 'yyyy-MM-dd HH:mm:ss'), 'seconds') <= 600");
		var resultAsTimeSpan = result as bool?;
		Assert.True(resultAsTimeSpan.HasValue);
		Assert.False(resultAsTimeSpan);
	}

	[Fact]
	public void TimeTest()
		=> DateTime.Parse("2022-01-24 17:04").ToString(":m").Should().Be(":4");
}
