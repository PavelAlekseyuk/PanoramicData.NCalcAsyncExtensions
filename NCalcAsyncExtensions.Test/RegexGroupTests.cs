namespace NCalcAsyncExtensions.Test;

public class RegexGroupTests : NCalcTest
{
	[Fact]
	public async Task RegexGroup_Simple_Succeeds()
	{
		var result = await TestAsync("regexGroup('abc:def:2019-01-01', '^.+?:.+?:(.+)$')");
		Assert.Equal("2019-01-01", result);
	}

	[Fact]
	public async Task RegexGroup_Succeeds()
	{
		var result = await TestAsync("regexGroup('abc:def:2019-01-01', '^.+?:.+?:(.+)$') < dateTime('UTC', 'yyyy-MM-dd', -30, 0, 0, 0)");
		Assert.NotNull(result as bool?);
		Assert.True((bool)result);
	}

	[Fact]
	public async Task RegexGroupN_Succeeds()
	{
		var result = await TestAsync("regexGroup('abc:def:XYZ', '^.+?:.+?:(.)+$')");
		Assert.Equal("X", result);
		result = await TestAsync("regexGroup('abc:def:XYZ', '^.+?:.+?:(.)+$', 1)");
		Assert.Equal("Y", result);
		result = await TestAsync("regexGroup('abc:def:XYZ', '^.+?:.+?:(.)+$', 2)");
		Assert.Equal("Z", result);
		result = await TestAsync("regexGroup('abc:def:XYZ', '^.+?:.+?:(.)+$', 3)");
		Assert.Null(result);
	}

	[Fact]
	public async Task RegexGroup_NotSoSimple_Succeeds()
	{
		var text = "AutoTask: xxx.xxx@xxx.com at 2022-05-10 08:26:57 UTC (37407238)\nTitle: Notification Description: xxx@xxx.xxx";
		var expression = new ExtendedExpression("regexGroup(text, 'AutoTask: .+ \\\\((\\\\d+)\\\\)\\\\n')");
		expression.Parameters["text"] = text;
		var result = await expression.EvaluateAsync();

		Assert.Equal("37407238", result);
	}

}
