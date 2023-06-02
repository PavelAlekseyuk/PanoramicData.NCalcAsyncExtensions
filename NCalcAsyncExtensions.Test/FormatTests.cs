namespace NCalcAsyncExtensions.Test;

public class FormatTests
{
	[Fact]
	public async Task Format_InvalidFormat_Fails()
	{
		var expression = new ExtendedExpression("format(1, 1)");
		var exception = await Assert.ThrowsAsync<ArgumentException>(expression.EvaluateAsync);
		exception.Message.Should().Be("format function - expected second argument to be a format string");
	}

	[Theory]
	[InlineData("format()")]
	[InlineData("format(1)")]
	public async Task Format_NotTwoParameters_Fails(string inputText)
	{
		var expression = new ExtendedExpression(inputText);
		var exception = await Assert.ThrowsAsync<ArgumentException>(expression.EvaluateAsync);
		exception.Message.Should().Be("format function - expected between 2 and 3 arguments");
	}

	[Theory]
	[InlineData("format(1, 2, 3)")]
	public async Task Format_ThreeParametersForInt_Fails(string inputText)
	{
		var expression = new ExtendedExpression(inputText);
		var exception = await Assert.ThrowsAsync<ArgumentException>(expression.EvaluateAsync);
		exception.Message.Should().Be("format function - expected second argument to be a format string");
	}

	[Fact]
	public async Task Format_IntFormat_Succeeds()
	{
		var expression = new ExtendedExpression("format(1, '00')");
		(await expression.EvaluateAsync()).Should().Be("01");
	}

	[Fact]
	public async Task Format_DoubleFormat_Succeeds()
	{
		var expression = new ExtendedExpression("format(1.0, '00')");
		(await expression.EvaluateAsync()).Should().Be("01");
	}

	[Fact]
	public async Task Format_DateTimeFormat_Succeeds()
	{
		var expression = new ExtendedExpression("format(dateTime('UTC', 'yyyy-MM-dd'), 'yyyy-MM-dd')");
		(await expression.EvaluateAsync()).Should().Be(DateTime.UtcNow.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
	}

	[Fact]
	public async Task Format_DateTimeFormatWithTimeZone_Succeeds()
	{
		var expression = new ExtendedExpression("format(theDateTime, 'yyyy-MM-dd HH:mm', 'Eastern Standard Time')");
		expression.Parameters.Add("theDateTime", DateTime.Parse("2020-03-13 16:00", CultureInfo.InvariantCulture));
		(await expression.EvaluateAsync()).Should().Be("2020-03-13 12:00");
	}

	[Fact]
	public async Task Format_StringFormat_Succeeds()
	{
		var expression = new ExtendedExpression("format('02', '0')");
		(await expression.EvaluateAsync()).Should().Be("2");
	}

	[Fact]
	public async Task Format_DateFormat_DayOfYear_Succeeds()
	{
		var expression = new ExtendedExpression("format('2021-11-29', 'dayOfYear')");
		(await expression.EvaluateAsync()).Should().Be("333");
	}

	/// <summary>
	///  See https://cdn.a-printable-calendar.com/images/large/full-year-calendar-2021.png and 
	///  See https://cdn.a-printable-calendar.com/images/large/full-year-calendar-2022.png
	/// </summary>
	/// <param name="dateTimeString"></param>
	/// <param name="expectedWeekOfMonth"></param>
	[Theory(DisplayName = "The numeric week of month as would be shown on a calendar with one row per week with weeks starting on a Sunday")]
	[InlineData("2021-11-01", 1)]
	[InlineData("2021-11-02", 1)]
	[InlineData("2021-11-03", 1)]
	[InlineData("2021-11-04", 1)]
	[InlineData("2021-11-05", 1)]
	[InlineData("2021-11-06", 1)]
	[InlineData("2021-11-07", 2)]
	[InlineData("2021-11-08", 2)]
	[InlineData("2021-11-09", 2)]
	[InlineData("2021-11-10", 2)]
	[InlineData("2021-11-11", 2)]
	[InlineData("2021-11-12", 2)]
	[InlineData("2021-11-13", 2)]
	[InlineData("2021-11-14", 3)]
	[InlineData("2021-11-15", 3)]
	[InlineData("2021-11-16", 3)]
	[InlineData("2021-11-17", 3)]
	[InlineData("2021-11-19", 3)]
	[InlineData("2021-11-20", 3)]
	[InlineData("2021-11-21", 4)]
	[InlineData("2021-11-22", 4)]
	[InlineData("2021-11-23", 4)]
	[InlineData("2021-11-24", 4)]
	[InlineData("2021-11-25", 4)]
	[InlineData("2021-11-26", 4)]
	[InlineData("2021-11-27", 4)]
	[InlineData("2021-11-28", 5)]
	[InlineData("2021-11-29", 5)]
	[InlineData("2021-11-30", 5)]
	[InlineData("2022-02-09", 2)]
	public async Task Format_DateFormat_WeekOfMonth_Succeeds(string dateTimeString, int expectedWeekOfMonth)
	{
		var expression = new ExtendedExpression($"format('{dateTimeString}', 'weekOfMonth')");
		(await expression.EvaluateAsync()).Should().Be(expectedWeekOfMonth.ToString(CultureInfo.InvariantCulture));
	}

	[Theory(DisplayName = "weekDayOfMonth calculates the number of times (including this time) that the day of week has occurred so far.")]
	[InlineData("2021-11-28", 4)] // This is in week 5 and is the 4th Sunday
	[InlineData("2021-11-30", 5)] // This is in week 5 and is the 5th Tuesday
	[InlineData("2022-02-09", 2)] // This is in week 2 and is the 2nd Wednesday
	public async Task Format_DateFormat_WeekDayOfMonth_Succeeds(string dateTimeString, int expectedWeekDayOfMonth)
	{
		var expression = new ExtendedExpression($"format('{dateTimeString}', 'weekDayOfMonth')");
		(await expression.EvaluateAsync()).Should().Be(expectedWeekDayOfMonth.ToString(CultureInfo.InvariantCulture));
	}

	[Fact]
	public async Task Format_DateFormat_WeekOfMonthText_Succeeds()
	{
		var expression = new ExtendedExpression("format('2021-11-30', 'weekOfMonthText')");
		(await expression.EvaluateAsync()).Should().Be("last");
	}

	[Fact]
	public async Task Format_DateTimeStringFormat_Succeeds()
	{
		var expression = new ExtendedExpression("format('01/01/2019', 'yyyy-MM-dd')");
		(await expression.EvaluateAsync()).Should().Be("2019-01-01");
	}

	[Fact]
	public async Task Format_InvalidStringFormat_Succeeds()
	{
		var expression = new ExtendedExpression("format('XXX', 'yyyy-MM-dd')");
		var exception = await Assert.ThrowsAsync<FormatException>(expression.EvaluateAsync);
		exception.Message.Should().Be("Could not parse 'XXX' as a number or date.");
	}

	[Fact]
	public async Task Format_SingleH_Succeeds()
	{
		var expression = new ExtendedExpression("parseInt(format(toDateTime((dateTimeAsEpochMs('2021-01-17 12:45:00', 'yyyy-MM-dd HH:mm:ss')) + (dateTimeAsEpochMs('1970-01-01 08:00:00', 'yyyy-MM-dd HH:mm:ss')), 'ms', 'UTC'), 'HH'))");
		var result = await expression.EvaluateAsync();
		result.Should().Be(20);
	}
}
