using System.Collections.Generic;

namespace NCalcAsyncExtensions.Test;

public class TimeSpanTests : NCalcTest
{
	private const string Date1AsString = "2020-01-01 00:00";
	private const string Date2AsString = "2020-03-01 02:34:56";

	public static IEnumerable<object[]> GetTestCases()
	{
		var expectedTimeSpan = DateTime.Parse(Date2AsString) - DateTime.Parse(Date1AsString);

		yield return new object[] { "Years", 0.1645656196922453 };
		yield return new object[] { "Weeks", 8.586798941798943 };
		yield return new object[] { "Days", 60.107592592592596 };
		yield return new object[] { "Hours", 1442.5822222222223 };
		yield return new object[] { "Minutes", 86554.93333333333 };
		yield return new object[] { "Seconds", 5193296.0 };
		yield return new object[] { "Milliseconds", 5193296000.0 };
		yield return new object[] { "c", expectedTimeSpan.ToString("c") };
		yield return new object[] { "g", expectedTimeSpan.ToString("g") };
		yield return new object[] { "G", expectedTimeSpan.ToString("G") };
		yield return new object[] { "hh", expectedTimeSpan.ToString("hh") };
		yield return new object[] { "mm", expectedTimeSpan.ToString("mm") };
		yield return new object[] { "ss", expectedTimeSpan.ToString("ss") };
	}

	[Theory]
	[MemberData(nameof(GetTestCases))]
	public async Task Timespan_Succeeds(string format, object expectedValue)
	{
		var result = await TestAsync($"timespan('{Date1AsString}', '{Date2AsString}', '{format}')");
		result.Should().Be(expectedValue);
	}
}
