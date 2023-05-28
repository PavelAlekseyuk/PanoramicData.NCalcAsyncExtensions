﻿using System.Threading.Tasks;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class ToDateTime
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		var argument1 = functionArgs.Parameters.Length >= 1 ? await functionArgs.Parameters[0].EvaluateAsync() : null;
		var argument2 = functionArgs.Parameters.Length >= 2 ? await functionArgs.Parameters[1].EvaluateAsync() : null;
		var argument3 = functionArgs.Parameters.Length >= 3 ? await functionArgs.Parameters[2].EvaluateAsync() : null;

		functionArgs.Result = (argument1, argument2, argument3) switch
		{
			// Only one argument
			(_, null, null) => throw new ArgumentException($"{ExtensionFunction.ToDateTime} function - Expected between 2 and 3 arguments"),

			// String and format, no timezone
			(string dateTimeString, string format, null) => DateTime.TryParseExact(dateTimeString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDateTime)
				? parsedDateTime
				: throw new ArgumentException($"{ExtensionFunction.ToDateTime} function - Input string did not match expected format."),

			// String, format and timezone
			(string dateTimeString, string format, string timeZoneName) => GetFromString(dateTimeString, format, timeZoneName),

			// long, format and timezone
			// Format can be:
			// - 's' (meaning seconds since the epoch)
			// - 'ms' (meaning milliseconds since the epoch)
			// - 'us' (meaning microseconds since the epoch)
			(long countSinceEpoch, string format, string timeZoneName) => GetFromDouble(countSinceEpoch, format, timeZoneName),
			(int countSinceEpoch, string format, string timeZoneName) => GetFromDouble(countSinceEpoch, format, timeZoneName),
			(double countSinceEpoch, string format, string timeZoneName) => GetFromDouble(countSinceEpoch, format, timeZoneName),

			// DateTime, and timezone, no third argument
			(DateTime dateTime, string timeZoneName, null) => GetFromDateTime(dateTime, timeZoneName),

			// Null first parameter should return null
			(null, _, _) => null,

			// Non-string format
			(string _, _, _) => throw new ArgumentException($"{ExtensionFunction.ToDateTime} function - Expected second argument to be a string."),
			(DateTime _, _, _) => throw new ArgumentException($"{ExtensionFunction.ToDateTime} function - Expected second argument to be a string."),

			_ => throw new ArgumentException($"{ExtensionFunction.ToDateTime} function - Expected first argument to be a long, int, double, string or DateTime.")
		};
	}

	private static object GetFromDateTime(DateTime dateTime, string timeZoneName)
	{
		var timeZoneInfo = TZConvert.GetTimeZoneInfo(timeZoneName);
		var dateTimeOffset = new DateTimeOffset(dateTime, timeZoneInfo.GetUtcOffset(dateTime));
		return dateTimeOffset.UtcDateTime;
	}

	private static object GetFromString(string dateTimeString, string format, string timeZoneName)
	{
		if (!DateTime.TryParseExact(dateTimeString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDateTime))
		{
			throw new ArgumentException($"{ExtensionFunction.ToDateTime} function - Input string did not match expected format.");
		}

		return ConvertTimeZone(parsedDateTime, timeZoneName, "UTC");
	}

	private static object GetFromDouble(double countSinceEpoch, string format, string timeZoneName)
	{
		var millisecondsSinceTheEpoch = format switch
		{
			"us" => countSinceEpoch / 1000,
			"ms" => countSinceEpoch,
			"s" => countSinceEpoch * 1000,
			_ => throw new ArgumentException("Format should relate to time since the epoch be one of 's' (seconds), 'ms' (milliseconds) or 'us' (microseconds)", nameof(format))
		};

		var parsedDateTime = new DateTime(1970, 1, 1).AddMilliseconds(millisecondsSinceTheEpoch);

		return ConvertTimeZone(parsedDateTime, timeZoneName, "UTC");
	}

	internal static object ConvertTimeZone(DateTime parsedDateTime, string sourceTimeZoneName, string destinationTimeZoneName)
	{
		var sourceTimeZoneInfo = TZConvert.GetTimeZoneInfo(sourceTimeZoneName);
		var destinationTimeZoneInfo = TZConvert.GetTimeZoneInfo(destinationTimeZoneName);
		return TimeZoneInfo.ConvertTime(parsedDateTime, sourceTimeZoneInfo, destinationTimeZoneInfo);
	}
}
