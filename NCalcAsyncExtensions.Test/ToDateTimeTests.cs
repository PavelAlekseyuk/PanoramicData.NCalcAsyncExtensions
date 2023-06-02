namespace NCalcAsyncExtensions.Test;

public class ToDateTimeTests : NCalcTest
{
	[Fact]
	public async Task StandardConversionNonDst_Succeeds()
	{
		const string format = "yyyy-MM-dd HH:mm";
		var result = await TestAsync($"toDateTime('2020-02-29 12:00', '{format}')");
		result.Should().Be(new DateTime(2020, 02, 29, 12, 00, 00));
	}

	[Fact]
	public async Task SingleParameter_Fails()
	{
		var expression = new ExtendedExpression("toDateTime('2020-02-29 12:00')");
		await Assert.ThrowsAsync<ArgumentException>(() => expression.EvaluateAsync());
	}

	[Fact]
	public async Task StandardConversionDst_Succeeds()
	{
		const string format = "yyyy-MM-dd HH:mm";
		var result = await TestAsync($"toDateTime('2020-06-06 12:00', '{format}')");
		result.Should().Be(new DateTime(2020, 06, 06, 12, 00, 00));
	}

	[Fact]
	public async Task TimeZoneConversion_Succeeds()
	{
		const string format = "yyyy-MM-dd HH:mm";
		var result = await TestAsync($"toDateTime('2020-02-29 12:00', '{format}', 'Eastern Standard Time')");
		result.Should().Be(new DateTime(2020, 02, 29, 17, 00, 00));
	}

	[Fact]
	public async Task TimeZoneDuringStupidTimeConversion_Succeeds()
	{
		const string format = "yyyy-MM-dd HH:mm";
		var result = await TestAsync($"toDateTime('2020-03-13 12:00', '{format}', 'Eastern Standard Time')");
		result.Should().Be(new DateTime(2020, 03, 13, 16, 00, 00));
	}

	[Fact]
	public async Task DateTimeFirstParameterWithTimeZone_Succeeds()
	{
		var estDateTime = new DateTime(2020, 03, 02, 12, 00, 00);
		var expression = new ExtendedExpression("toDateTime(estDateTime, 'Eastern Standard Time')");
		expression.Parameters[nameof(estDateTime)] = estDateTime;
		var utcDateTime = await expression.EvaluateAsync();
		utcDateTime.Should().Be(new DateTime(2020, 03, 02, 17, 00, 00));
	}

	[Fact]
	public async Task NullFirstParameterWithTimeZone_Succeeds()
	{
		object? estDateTime = null;
		var expression = new ExtendedExpression("toDateTime(estDateTime, 'Eastern Standard Time')");
		expression.Parameters[nameof(estDateTime)] = estDateTime;
		var utcDateTime = await expression.EvaluateAsync();
		utcDateTime.Should().BeNull();
	}

	[Fact]
	public async Task DateTimeFirstParameterWithoutTimeZone_Fails()
	{
		var estDateTime = new DateTime(2020, 03, 02, 12, 00, 00);
		var expression = new ExtendedExpression("toDateTime(theDateTime)");
		expression.Parameters[nameof(estDateTime)] = estDateTime;
		await Assert.ThrowsAsync<ArgumentException>(() => expression.EvaluateAsync());
	}

	[Fact]
	public async Task DateTimeIntSeconds_Succeeds()
	{
		var expectedDateTime = new DateTime(1975, 02, 17, 00, 00, 00);
		var expression = new ExtendedExpression("toDateTime(161827200.0, 's', 'UTC')");
		expression.Parameters[nameof(expectedDateTime)] = expectedDateTime;
		(await expression.EvaluateAsync()).Should().Be(expectedDateTime);
	}

	[Fact]
	public async Task DateTimeLongMilliseconds_Succeeds()
	{
		var expectedDateTime = new DateTime(1975, 02, 17, 00, 00, 00);
		var expression = new ExtendedExpression("toDateTime(161827200000.0, 'ms', 'UTC')");
		expression.Parameters[nameof(expectedDateTime)] = expectedDateTime;
		(await expression.EvaluateAsync()).Should().Be(expectedDateTime);
	}

	[Fact]
	public async Task DateTimeLongMicroseconds_Succeeds()
	{
		var expectedDateTime = new DateTime(1975, 02, 17, 00, 00, 00);
		var expression = new ExtendedExpression("toDateTime(161827200000000.0, 'us', 'UTC')");
		expression.Parameters[nameof(expectedDateTime)] = expectedDateTime;
		(await expression.EvaluateAsync()).Should().Be(expectedDateTime);
	}
}
