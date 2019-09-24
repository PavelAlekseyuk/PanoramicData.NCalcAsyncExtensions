﻿using NCalc;
using PanoramicData.NCalcExtensions.Exceptions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace PanoramicData.NCalcExtensions
{
	public static class NCalcExtensions
	{
		[SuppressMessage("Design", "RCS1224:Make method an extension method.", Justification = "Nonsense")]
		public static void Extend(string functionName, FunctionArgs functionArgs)
		{
			string param1;
			string param2;
			switch (functionName)
			{
				case "cast":
					{
						const int castParameterCount = 2;
						if (functionArgs.Parameters.Length != castParameterCount)
						{
							throw new ArgumentException($"Expected {castParameterCount} arguments");
						}
						var inputObject = functionArgs.Parameters[0].Evaluate();
						if (!(functionArgs.Parameters[1].Evaluate() is string castTypeString))
						{
							throw new ArgumentException("Expected second argument to be a string.");
						}
						var castType = Type.GetType(castTypeString);
						if (castType == null)
						{
							throw new ArgumentException("Expected second argument to be a valid .NET type e.g. System.Decimal.");
						}
						var result = Convert.ChangeType(inputObject, castType);
						functionArgs.Result = result;
						return;
					}
				case "dateTimeAsEpochMs":
					var dateTimeOffset = DateTimeOffset.ParseExact(
						functionArgs.Parameters[0].Evaluate() as string, // Input date as string
						functionArgs.Parameters[1].Evaluate() as string,
						CultureInfo.InvariantCulture.DateTimeFormat,
						DateTimeStyles.AssumeUniversal);
					functionArgs.Result = dateTimeOffset.ToUnixTimeMilliseconds();
					break;
				case "dateTime":
					// Time Zone
					string timeZone;
					if (functionArgs.Parameters.Length > 0)
					{
						timeZone = functionArgs.Parameters[0].Evaluate() as string;
						if (timeZone == null)
						{
							throw new FormatException("The first argument should be a string, e.g. 'UTC'");
						}
						// TODO - support more than just UTC
						if (timeZone != "UTC")
						{
							throw new FormatException("Only UTC timeZone is currently supported.");
						}
					}
					else
					{
						timeZone = "UTC";
					}
					// Time zone has been determined

					// Format
					string format;
					if (functionArgs.Parameters.Length > 1)
					{
						format = functionArgs.Parameters[1].Evaluate() as string;
					}
					else
					{
						format = "YYYY-MM-dd HH:mm:ss";
					}
					// Format has been determined

					// Days to add
					double daysToAdd = 0;
					if (functionArgs.Parameters.Length > 2)
					{
						var daysToAddNullable = GetNullableDouble(functionArgs.Parameters[2]);
						if (!daysToAddNullable.HasValue)
						{
							throw new FormatException("Days to add must be a number.");
						}
						daysToAdd = daysToAddNullable.Value;
					}

					// Hours to add
					double hoursToAdd = 0;
					if (functionArgs.Parameters.Length > 3)
					{
						var hoursToAddNullable = GetNullableDouble(functionArgs.Parameters[3]);
						if (!hoursToAddNullable.HasValue)
						{
							throw new FormatException("Hours to add must be a number.");
						}
						hoursToAdd = hoursToAddNullable.Value;
					}

					// Minutes to add
					double minutesToAdd = 0;
					if (functionArgs.Parameters.Length > 4)
					{
						var minutesToAddNullable = GetNullableDouble(functionArgs.Parameters[4]);
						if (!minutesToAddNullable.HasValue)
						{
							throw new FormatException("Minutes to add must be a number.");
						}
						minutesToAdd = minutesToAddNullable.Value;
					}

					// Seconds to add
					double secondsToAdd = 0;
					if (functionArgs.Parameters.Length > 5)
					{
						var secondsToAddNullable = GetNullableDouble(functionArgs.Parameters[5]);
						if (!secondsToAddNullable.HasValue)
						{
							throw new FormatException("Seconds to add must be a number.");
						}
						secondsToAdd = secondsToAddNullable.Value;
					}

					functionArgs.Result = DateTimeOffset
						.UtcNow
						.AddDays(daysToAdd)
						.AddHours(hoursToAdd)
						.AddMinutes(minutesToAdd)
						.AddSeconds(secondsToAdd)
						.ToString(format);
					return;
				case "endsWith":
					try
					{
						param1 = (string)functionArgs.Parameters[0].Evaluate();
						param2 = (string)functionArgs.Parameters[1].Evaluate();
					}
					catch (NCalcExtensionsException)
					{
						throw;
					}
					catch (Exception)
					{
						throw new FormatException("endsWith() requires two string parameters.");
					}
					functionArgs.Result = param1.EndsWith(param2, StringComparison.InvariantCulture);
					return;
				case "format":
					{
						if (functionArgs.Parameters.Length != 2)
						{
							throw new ArgumentException("Expected two arguments");
						}

						if (!(functionArgs.Parameters[1].Evaluate() is string formatFormat))
						{
							throw new ArgumentException("Expected second argument to be a format string");
						}

						var inputObject = functionArgs.Parameters[0].Evaluate();
						switch (inputObject)
						{
							case int inputInt:
								functionArgs.Result = inputInt.ToString(formatFormat);
								return;
							case double inputDouble:
								functionArgs.Result = inputDouble.ToString(formatFormat);
								return;
							case DateTime dateTime:
								functionArgs.Result = dateTime.ToString(formatFormat);
								return;
							case string inputString:
								// Assume this is a number
								if (long.TryParse(inputString, out var longValue))
								{
									functionArgs.Result = longValue.ToString(formatFormat);
									return;
								}
								if (double.TryParse(inputString, out var doubleValue))
								{
									functionArgs.Result = doubleValue.ToString(formatFormat);
									return;
								}
								if (DateTimeOffset.TryParse(
									inputString,
									CultureInfo.InvariantCulture.DateTimeFormat,
									DateTimeStyles.AssumeUniversal,
									out var dateTimeOffsetValue))
								{
									functionArgs.Result = dateTimeOffsetValue.ToString(formatFormat);
									return;
								}
								throw new FormatException($"Could not parse '{inputString}' as a number or date.");
							default:
								throw new NotSupportedException($"Unsupported input type {inputObject.GetType().Name}");
						}
					}
				case "in":
					if (functionArgs.Parameters.Length < 2)
					{
						throw new FormatException("in() requires at least two parameters.");
					}
					try
					{
						var item = functionArgs.Parameters[0].Evaluate();
						var list = functionArgs.Parameters.Skip(1).Select(p => p.Evaluate()).ToList();
						functionArgs.Result = list.Contains(item);
						return;
					}
					catch (NCalcExtensionsException)
					{
						throw;
					}
					catch (Exception)
					{
						throw new FormatException("in() parameters malformed.");
					}
				case "if":
					bool boolParam1;
					if (functionArgs.Parameters.Length != 3)
					{
						throw new FormatException("if() requires three parameters.");
					}
					try
					{
						boolParam1 = (bool)functionArgs.Parameters[0].Evaluate();
					}
					catch (NCalcExtensionsException)
					{
						throw;
					}
					catch (Exception)
					{
						throw new FormatException($"Could not evaluate if function parameter 1 '{functionArgs.Parameters[0].ParsedExpression}'.");
					}
					if (boolParam1)
					{
						try
						{
							functionArgs.Result = functionArgs.Parameters[1].Evaluate();
							return;
						}
						catch (NCalcExtensionsException)
						{
							throw;
						}
						catch (Exception e)
						{
							throw new FormatException($"Could not evaluate if function parameter 2 '{functionArgs.Parameters[1].ParsedExpression}' due to {e.Message}.", e);
						}
					}
					try
					{
						functionArgs.Result = functionArgs.Parameters[2].Evaluate();
						return;
					}
					catch (NCalcExtensionsException)
					{
						throw;
					}
					catch (Exception e)
					{
						throw new FormatException($"Could not evaluate if function parameter 3 '{functionArgs.Parameters[2].ParsedExpression}' due to {e.Message}.", e);
					}
				case "isInfinite":
					if (functionArgs.Parameters.Length != 1)
					{
						throw new FormatException("isInfinite() requires one parameter.");
					}
					try
					{
						var outputObject = functionArgs.Parameters[0].Evaluate();
						functionArgs.Result =
							outputObject is double x && (
								double.IsPositiveInfinity(x)
								|| double.IsNegativeInfinity(x)
							);
						return;
					}
					catch (NCalcExtensionsException)
					{
						throw;
					}
					catch (FormatException)
					{
						throw;
					}
					catch (Exception e)
					{
						throw new FormatException(e.Message);
					}
				case "isNaN":
					if (functionArgs.Parameters.Length != 1)
					{
						throw new FormatException("isNaN() requires one parameter.");
					}
					try
					{
						var outputObject = functionArgs.Parameters[0].Evaluate();
						functionArgs.Result = !(outputObject is double) || double.IsNaN((double)outputObject);
						return;
					}
					catch (NCalcExtensionsException)
					{
						throw;
					}
					catch (FormatException)
					{
						throw;
					}
					catch (Exception e)
					{
						throw new FormatException(e.Message);
					}
				case "isNull":
					if (functionArgs.Parameters.Length != 1)
					{
						throw new FormatException("isNull() requires one parameter.");
					}
					try
					{
						var outputObject = functionArgs.Parameters[0].Evaluate();
						functionArgs.Result = outputObject == null;
						return;
					}
					catch (NCalcExtensionsException)
					{
						throw;
					}
					catch (FormatException)
					{
						throw;
					}
					catch (Exception e)
					{
						throw new FormatException(e.Message);
					}
				case "contains":
					try
					{
						param1 = (string)functionArgs.Parameters[0].Evaluate();
						param2 = (string)functionArgs.Parameters[1].Evaluate();
					}
					catch (NCalcExtensionsException)
					{
						throw;
					}
					catch (Exception)
					{
						throw new FormatException("contains() requires two string parameters.");
					}
					functionArgs.Result = param1.IndexOf(param2, StringComparison.InvariantCulture) >= 0;
					return;
				case "indexOf":
					try
					{
						param1 = (string)functionArgs.Parameters[0].Evaluate();
						param2 = (string)functionArgs.Parameters[1].Evaluate();
					}
					catch (NCalcExtensionsException)
					{
						throw;
					}
					catch (Exception)
					{
						throw new FormatException("indexOf() requires two string parameters.");
					}
					functionArgs.Result = param1.IndexOf(param2, StringComparison.InvariantCulture);
					return;
				case "lastIndexOf":
					try
					{
						param1 = (string)functionArgs.Parameters[0].Evaluate();
						param2 = (string)functionArgs.Parameters[1].Evaluate();
					}
					catch (NCalcExtensionsException)
					{
						throw;
					}
					catch (Exception)
					{
						throw new FormatException("lastIndexOf() requires two string parameters.");
					}
					functionArgs.Result = param1.LastIndexOf(param2, StringComparison.InvariantCulture);
					return;
				case "length":
					try
					{
						param1 = (string)functionArgs.Parameters[0].Evaluate();
					}
					catch (NCalcExtensionsException)
					{
						throw;
					}
					catch (Exception)
					{
						throw new FormatException("length() requires one string parameter.");
					}
					functionArgs.Result = param1.Length;
					return;
				case "startsWith":
					if (functionArgs.Parameters.Length != 2)
					{
						throw new FormatException("startsWith() requires two parameters.");
					}
					try
					{
						param1 = (string)functionArgs.Parameters[0].Evaluate() as string;
						param2 = (string)functionArgs.Parameters[1].Evaluate() as string;
					}
					catch (NCalcExtensionsException)
					{
						throw;
					}
					catch (Exception e)
					{
						throw new FormatException($"Unexpected exception in startsWith(): {e.Message}", e);
					}
					if (param1 == null)
					{
						throw new FormatException($"startsWith() parameter 1 is not a string");
					}
					if (param2 == null)
					{
						throw new FormatException($"startsWith() parameter 2 is not a string");
					}
					functionArgs.Result = param1.StartsWith(param2, StringComparison.InvariantCulture);
					return;
				case "regexGroup":
					try
					{
						var input = (string)functionArgs.Parameters[0].Evaluate();
						var regexExpression = (string)functionArgs.Parameters[1].Evaluate();
						var regexCaptureIndex = functionArgs.Parameters.Length == 3
							? (int)functionArgs.Parameters[2].Evaluate()
							: 0;
						var regex = new Regex(regexExpression);
						if (!regex.IsMatch(input))
						{
							functionArgs.Result = null;
						}
						else
						{
							Group group = regex
								.Match(input)
								.Groups[1];
							functionArgs.Result = regexCaptureIndex >= group.Captures.Count
								? null
								: group.Captures[regexCaptureIndex].Value;
						}
					}
					catch (NCalcExtensionsException)
					{
						throw;
					}
					catch (Exception)
					{
						throw new FormatException("replace() requires three string parameters.");
					}
					return;
				case "regexIsMatch":
					try
					{
						var input = (string)functionArgs.Parameters[0].Evaluate();
						var regexExpression = (string)functionArgs.Parameters[1].Evaluate();
						var regex = new Regex(regexExpression);
						functionArgs.Result = regex.IsMatch(input);
					}
					catch (NCalcExtensionsException)
					{
						throw;
					}
					catch (Exception)
					{
						throw new FormatException("replace() requires three string parameters.");
					}
					return;
				case "replace":
					try
					{
						var haystack = (string)functionArgs.Parameters[0].Evaluate();
						var needle = (string)functionArgs.Parameters[1].Evaluate();
						var newNeedle = (string)functionArgs.Parameters[2].Evaluate();
						functionArgs.Result = haystack.Replace(needle, newNeedle);
					}
					catch (NCalcExtensionsException)
					{
						throw;
					}
					catch (Exception)
					{
						throw new FormatException("replace() requires three string parameters.");
					}
					return;
				case "substring":
					int startIndex;
					int length;
					try
					{
						param1 = (string)functionArgs.Parameters[0].Evaluate();
						startIndex = (int)functionArgs.Parameters[1].Evaluate();
					}
					catch (NCalcExtensionsException)
					{
						throw;
					}
					catch (Exception)
					{
						throw new FormatException("substring() requires a string parameter and one or two numeric parameters.");
					}
					if (functionArgs.Parameters.Length > 2)
					{
						length = (int)functionArgs.Parameters[2].Evaluate();
						functionArgs.Result = param1.Substring(startIndex, length);
						return;
					}

					functionArgs.Result = param1.Substring(startIndex);
					return;
				case "throw":
					switch (functionArgs.Parameters.Length)
					{
						case 0:
							throw new NCalcExtensionsException();
						case 1:
							if (!(functionArgs.Parameters[0].Evaluate() is string exceptionMessageText))
							{
								throw new FormatException("throw() parameter must be a string.");
							}
							throw new NCalcExtensionsException(exceptionMessageText);

						default:
							throw new FormatException("throw() takes zero or one parameters.");
					}
				case "timeSpan":
					if (functionArgs.Parameters.Length != 3)
					{
						throw new FormatException("timeSpan() requires three parameters.");
					}
					string fromString;
					string toString;
					string timeUnitString;
					try
					{
						fromString = functionArgs.Parameters[0].Evaluate().ToString();
						toString = functionArgs.Parameters[1].Evaluate().ToString();
						timeUnitString = functionArgs.Parameters[2].Evaluate().ToString();
					}
					catch (NCalcExtensionsException)
					{
						throw;
					}
					catch (Exception e)
					{
						throw new FormatException($"timeSpan() could not extract three parameters into strings: {e.Message}");
					}

					if (!DateTime.TryParse(fromString, out var fromDateTime))
					{
						throw new FormatException($"timeSpan() could not convert '{fromString}' to DateTime");
					}
					if (!DateTime.TryParse(toString, out var toDateTime))
					{
						throw new FormatException($"timeSpan() could not convert '{toString}' to DateTime");
					}
					if (!Enum.TryParse(timeUnitString, true, out TimeUnit timeUnit))
					{
						throw new FormatException($"timeSpan() could not convert '{timeUnitString}' to a supported time unit");
					}

					// Determine the timespan
					var timeSpan = toDateTime - fromDateTime;
					functionArgs.Result = GetUnits(timeSpan, timeUnit);
					return;
				case "toDateTime":
					{
						const int toDateTimeParameterCount = 2;
						if (functionArgs.Parameters.Length != toDateTimeParameterCount)
						{
							throw new ArgumentException($"Expected {toDateTimeParameterCount} arguments");
						}
						if (!(functionArgs.Parameters[0].Evaluate() is string inputString))
						{
							throw new ArgumentException("Expected first argument to be a string.");
						}
						if (!(functionArgs.Parameters[1].Evaluate() is string formatString))
						{
							throw new ArgumentException("Expected second argument to be a string.");
						}
						if (!DateTime.TryParseExact(inputString, formatString, CultureInfo.InvariantCulture, DateTimeStyles.None, out var outputDateTime))
						{
							throw new ArgumentException("Input string did not match expected format.");
						}
						functionArgs.Result = outputDateTime;
						return;
					}
				case "toLower":
					try
					{
						param1 = (string)functionArgs.Parameters[0].Evaluate();
					}
					catch (NCalcExtensionsException)
					{
						throw;
					}
					catch (Exception)
					{
						throw new FormatException("toLower() requires one string parameter.");
					}
					functionArgs.Result = param1.ToLowerInvariant();
					return;
				case "toUpper":
					try
					{
						param1 = (string)functionArgs.Parameters[0].Evaluate();
					}
					catch (NCalcExtensionsException)
					{
						throw;
					}
					catch (Exception)
					{
						throw new FormatException("toUpper() requires one string parameter.");
					}
					functionArgs.Result = param1.ToUpperInvariant();
					return;
				case "capitalize":
				case "capitalise":
					try
					{
						param1 = (string)functionArgs.Parameters[0].Evaluate();
					}
					catch (NCalcExtensionsException)
					{
						throw;
					}
					catch (Exception)
					{
						throw new FormatException("capitalise() requires one string parameter.");
					}
					functionArgs.Result = param1.ToLowerInvariant().UpperCaseFirst();
					return;
				case "humanize":
				case "humanise":
					double param1Double;
					try
					{
						if (double.TryParse(functionArgs.Parameters[0].Evaluate().ToString(), out var result))
						{
							param1Double = result;
							param2 = (string)functionArgs.Parameters[1].Evaluate();
						}
						else
						{
							throw new Exception();
						}
					}
					catch (NCalcExtensionsException)
					{
						throw;
					}
					catch (Exception)
					{
						throw new FormatException($"Incorrect usage of humanize(double value, string timeUnit).  The first number should be a valid floating-point number and the second should be a time unit ({string.Join(", ", Enum.GetNames(typeof(TimeUnit)))}).");
					}

					if (!Enum.TryParse<TimeUnit>(param2, true, out var param2TimeUnit))
					{
						throw new FormatException($"humanize() parameter 2 must be a time unit - one of {string.Join(", ", Enum.GetNames(typeof(TimeUnit)).Select(n => $"'{n}'"))}.");
					}

					functionArgs.Result = Humanize(param1Double, param2TimeUnit);
					return;
			}
		}

		private static double? GetNullableDouble(Expression expression)
		{
			switch (expression.Evaluate())
			{
				case double doubleResult:
					return (double?)doubleResult;
				case int intResult:
					return (double?)intResult;
				default:
					return null;
			}
		}

		public static string UpperCaseFirst(this string s)
			=> s.Substring(0, 1).ToUpperInvariant() + s.Substring(1);

		private static double GetUnits(TimeSpan timeSpan, TimeUnit timeUnit)
		{
			switch (timeUnit)
			{
				case TimeUnit.Milliseconds:
					return timeSpan.TotalMilliseconds;
				case TimeUnit.Seconds:
					return timeSpan.TotalSeconds;
				case TimeUnit.Minutes:
					return timeSpan.TotalMinutes;
				case TimeUnit.Hours:
					return timeSpan.TotalHours;
				case TimeUnit.Days:
					return timeSpan.TotalDays;
				case TimeUnit.Weeks:
					return timeSpan.TotalDays / 7;
				case TimeUnit.Years:
					return timeSpan.TotalDays / 365.25;
				default:
					throw new ArgumentOutOfRangeException($"Time unit not supported: '{timeUnit}'");
			}
		}

		[SuppressMessage("Design", "RCS1224:Make method an extension method.", Justification = "Nonsense")]
		internal static string Humanize(double param1Double, TimeUnit timeUnit)
		{
			try
			{
				switch (timeUnit)
				{
					case TimeUnit.Milliseconds:
						return TimeSpan.FromMilliseconds(param1Double).Humanize();
					case TimeUnit.Seconds:
						return TimeSpan.FromSeconds(param1Double).Humanize();
					case TimeUnit.Minutes:
						return TimeSpan.FromMinutes(param1Double).Humanize();
					case TimeUnit.Hours:
						return TimeSpan.FromHours(param1Double).Humanize();
					case TimeUnit.Days:
						return TimeSpan.FromDays(param1Double).Humanize();
					case TimeUnit.Weeks:
						return TimeSpan.FromDays(param1Double * 7).Humanize();
					case TimeUnit.Years:
						return TimeSpan.FromDays(param1Double * 365.25).Humanize();
					default:
						throw new FormatException($"{timeUnit} is not a supported time unit for humanization.");
				}
			}
			catch (OverflowException)
			{
				throw new FormatException("The value is too big to use humanize. It must be a double (a 64-bit, floating point number)");
			}
		}

		public static string Humanize(this TimeSpan t)
		{
			// Humanize a TimeSpan into days, hours, minutes and seconds.
			var durationString = t.Days >= 1 ? $"{t.Days} day{(t.Days > 1 ? "s" : "")}" : "";

			if (t.Hours >= 1)
			{
				durationString += $" {t.Hours} hour{(t.Hours > 1 ? "s" : "")}";
			}

			if (t.Minutes >= 1)
			{
				durationString += $" {t.Minutes} minute{(t.Minutes > 1 ? "s" : "")}";
			}

			if (t.Seconds >= 1)
			{
				durationString += $" {t.Seconds} second{(t.Seconds > 1 ? "s" : "")}";
			}

			return durationString.Trim();
		}
	}
}
