namespace PanoramicData.NCalcAsyncExtensions.Test;

public class RegexIsMatchTests : NCalcTest
{
	[Fact]
	public async Task RegexIsMatch_True_Succeeds()
	{
		var result = await TestAsync("regexIsMatch('abc:def:2019-01-01', '^.+?:.+?:(.+)$')");
		Assert.True((bool)result);
	}

	[Fact]
	public async Task RegexIsMatch_False_Succeeds()
	{
		var result = await TestAsync("regexIsMatch('YYYYYYYYYYY', '^XXXXXXXX$')");
		Assert.False((bool)result);
	}

	[Fact]
	public async Task RegexIsMatch_JObject_Succeeds()
	{
		var result = await TestAsync("regexIsMatch(jPath(jObject('name', 'UK123'), 'name'), '^UK')");
		Assert.True((bool)result);
	}
}
