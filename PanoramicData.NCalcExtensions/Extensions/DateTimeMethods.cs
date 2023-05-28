﻿namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class DateTimeMethods
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		if (functionArgs.Parameters.Length > 0)
		{
			// Time Zone
			if (await functionArgs.Parameters[0].EvaluateSafelyAsync() is not string timeZone)
			{
				throw new FormatException($"{ExtensionFunction.DateTime} function - The first argument should be a string, e.g. 'UTC'");
			}
			// TODO - support more than just UTC
			if (timeZone != "UTC")
			{
				throw new FormatException($"{ExtensionFunction.DateTime} function - Only UTC timeZone is currently supported.");
			}
		}
		// Time zone has been determined

		// Format
		var format = functionArgs.Parameters.Length > 1
			? await functionArgs.Parameters[1].EvaluateSafelyAsync() as string
			: "yyyy-MM-dd HH:mm:ss";
		// Format has been determined

		// Days to add
		double daysToAdd = 0;
		if (functionArgs.Parameters.Length > 2)
		{
			var daysToAddNullable = await GetNullableDoubleAsync(functionArgs.Parameters[2]);
			if (!daysToAddNullable.HasValue)
			{
				throw new FormatException($"{ExtensionFunction.DateTime} function - Days to add must be a number.");
			}

			daysToAdd = daysToAddNullable.Value;
		}

		// Hours to add
		double hoursToAdd = 0;
		if (functionArgs.Parameters.Length > 3)
		{
			var hoursToAddNullable = await GetNullableDoubleAsync(functionArgs.Parameters[3]);
			if (!hoursToAddNullable.HasValue)
			{
				throw new FormatException($"{ExtensionFunction.DateTime} function - Hours to add must be a number.");
			}

			hoursToAdd = hoursToAddNullable.Value;
		}

		// Minutes to add
		double minutesToAdd = 0;
		if (functionArgs.Parameters.Length > 4)
		{
			var minutesToAddNullable = await GetNullableDoubleAsync(functionArgs.Parameters[4]);
			if (!minutesToAddNullable.HasValue)
			{
				throw new FormatException($"{ExtensionFunction.DateTime} function - Minutes to add must be a number.");
			}

			minutesToAdd = minutesToAddNullable.Value;
		}

		// Seconds to add
		double secondsToAdd = 0;
		if (functionArgs.Parameters.Length > 5)
		{
			var secondsToAddNullable = await GetNullableDoubleAsync(functionArgs.Parameters[5]);
			if (!secondsToAddNullable.HasValue)
			{
				throw new FormatException($"{ExtensionFunction.DateTime} function - Seconds to add must be a number.");
			}

			secondsToAdd = secondsToAddNullable.Value;
		}

		functionArgs.Result = DateTimeOffset
			.UtcNow
			.AddDays(daysToAdd)
			.AddHours(hoursToAdd)
			.AddMinutes(minutesToAdd)
			.AddSeconds(secondsToAdd)
			.ToString(format, CultureInfo.InvariantCulture);
	}

	private static async Task<double?> GetNullableDoubleAsync(Expression expression)
		=> (await expression.EvaluateSafelyAsync()) switch
		{
			double doubleResult => doubleResult,
			int intResult => intResult,
			_ => null,
		};

	internal static string BetterToString(this DateTime dateTime, string format)
		=> format switch
		{
			"dayOfYear" => dateTime.DayOfYear.ToString(),
			"weekOfMonth" => dateTime.WeekOfMonth().ToString(),
			"weekOfMonthText" => GetWeekText(dateTime.WeekOfMonth()),
			"weekDayOfMonth" => dateTime.WeekDayOfMonth().ToString(),
			"weekDayOfMonthText" => GetWeekText(dateTime.WeekDayOfMonth()),
			_ => dateTime.ToString(format, CultureInfo.InvariantCulture)
		};

	private static string GetWeekText(int weekOfMonth) => weekOfMonth switch
	{
		1 => "first",
		2 => "second",
		3 => "third",
		4 => "fourth",
		_ => "last"
	};

	public static int WeekOfMonth(this DateTime dateTime)
	{
		var date = dateTime.Date;
		var firstOfMonth = new DateTime(date.Year, date.Month, 1);
		return ((date - firstOfMonth
			.AddDays(-(int)firstOfMonth.DayOfWeek)).Days / 7) + 1;
	}

	public static int WeekDayOfMonth(this DateTime dateTime)
		=> ((dateTime.Day - 1) / 7) + 1;

	internal static string ToDateTimeInTargetTimeZone(this DateTime dateTime, string formatFormat, string timeZoneString)
	{
		var timeZoneInfo = TZConvert.GetTimeZoneInfo(timeZoneString);
		var dateTimeOffset = new DateTimeOffset(dateTime, -timeZoneInfo.GetUtcOffset(dateTime));
		return dateTimeOffset.UtcDateTime.BetterToString(formatFormat);
	}
}
