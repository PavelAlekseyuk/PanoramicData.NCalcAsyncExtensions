﻿using NCalcAsync;
using PanoramicData.NCalcAsyncExtensions.Exceptions;
using System.Threading.Tasks;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class TimeSpan
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		if (functionArgs.Parameters.Length != 3)
		{
			throw new FormatException($"{ExtensionFunction.TimeSpan} function - requires three parameters.");
		}

		string fromString;
		string toString;
		string timeFormat;
		try
		{
			fromString = (await functionArgs.Parameters[0].EvaluateAsync()).ToString();
			toString = (await functionArgs.Parameters[1].EvaluateAsync()).ToString();
			timeFormat = (await functionArgs.Parameters[2].EvaluateAsync()).ToString();
		}
		catch (NCalcExtensionsException)
		{
			throw;
		}
		catch (Exception e)
		{
			throw new FormatException($"{ExtensionFunction.TimeSpan} function -  could not extract three parameters into strings: {e.Message}");
		}

		if (!System.DateTime.TryParse(fromString, out var fromDateTime))
		{
			throw new FormatException($"{ExtensionFunction.TimeSpan} function -  could not convert '{fromString}' to DateTime");
		}

		if (!System.DateTime.TryParse(toString, out var toDateTime))
		{
			throw new FormatException($"{ExtensionFunction.TimeSpan} function -  could not convert '{toString}' to DateTime");
		}

		// Determine the timespan
		var timeSpan = toDateTime - fromDateTime;

		functionArgs.Result = Enum.TryParse(timeFormat, true, out TimeUnit timeUnit)
			? GetUnits(timeSpan, timeUnit)
			: timeSpan.ToString(timeFormat);
	}

	private static object GetUnits(System.TimeSpan timeSpan, TimeUnit timeUnit)
		=> timeUnit switch
		{
			TimeUnit.Milliseconds => timeSpan.TotalMilliseconds,
			TimeUnit.Seconds => timeSpan.TotalSeconds,
			TimeUnit.Minutes => timeSpan.TotalMinutes,
			TimeUnit.Hours => timeSpan.TotalHours,
			TimeUnit.Days => timeSpan.TotalDays,
			TimeUnit.Weeks => timeSpan.TotalDays / 7,
			TimeUnit.Years => timeSpan.TotalDays / 365.25,
			_ => throw new ArgumentOutOfRangeException($"Time unit not supported: '{timeUnit}'"),
		};
}
