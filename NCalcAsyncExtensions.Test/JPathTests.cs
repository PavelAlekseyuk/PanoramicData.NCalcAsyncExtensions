﻿using Meraki.Api.Data;

namespace NCalcAsyncExtensions.Test;
public class JPathTests : NCalcTest
{
	private static readonly JObject TestJObject = JObject.Parse("{ \"name\": \"bob\", \"numbers\": [1, 2], \"kvps\": [ { \"key\": \"key1\", \"value\": \"value1\" }, { \"key\": \"key2\", \"value\": \"value2\" } ] }");

	[Theory]
	[InlineData("name", "bob")]
	[InlineData("numbers[0]", 1)]
	[InlineData("numbers[1]", 2)]
	[InlineData("kvps[?(@.key==\\'key1\\')].value", "value1")]
	[InlineData("kvps[?(@.key==\\'key2\\')].value", "value2")]
	public async Task JPath_ValidPath_Works(string path, object expectedResult)
	{
		var expression = new ExtendedExpression($"jPath(source, '{path}')");
		expression.Parameters["source"] = TestJObject;
		var result = await expression.EvaluateAsync();
		result.Should().Be(
			expectedResult,
			because: "the jPath is valid, present and returns a single result"
		);
	}

	[Theory]
	[InlineData("key1", "value1")]
	[InlineData("keyXXX", null)]
	public async Task JPath_DictionaryAccess_Works(string key, string? expectedValue)
	{
		var expression = new ExtendedExpression($"jPath(source, 'kvps[?(@key==\\'{key}\\')].value', true)");
		expression.Parameters["source"] = TestJObject;
		var result = await expression.EvaluateAsync();
		result.Should().Be(expectedValue);
	}

	[Fact]
	public async Task JPath_SourceObjectIsAString_NotAJObject()
	{
		var expression = new ExtendedExpression("jPath(source, 'name')");
		expression.Parameters["source"] = "SomeRandomString";
		var exception = await Assert.ThrowsAsync<FormatException>(expression.EvaluateAsync);
	}

	[Fact]
	public async Task JPath_SourceObjectIsAPoco_Succeeds()
	{
		var expression = new ExtendedExpression("jPath(source, 'name')");
		expression.Parameters["source"] = new Organization { Name = "woo" };
		var result = await expression.EvaluateAsync();
		result.Should().Be("woo");
	}

	[Fact]
	public async Task JPath_AccessArray_Succeeds()
	{
		var expression = new ExtendedExpression("jPath(source, 'kvps')");
		expression.Parameters["source"] = TestJObject;
		var result = await expression.EvaluateAsync();
		result.Should().BeOfType<JArray>();
	}

	[Fact]
	public async Task JPath_StringCompare_Succeeds()
	{
		var expression = new ExtendedExpression("jPath(source, 'name') == 'bob'");
		expression.Parameters["source"] = JObject.FromObject(new Organization { Name = "bob" });
		var result = await expression.EvaluateAsync();
		result.Should().BeOfType<bool>();
		result.Should().Be(true);
	}

	[Fact]
	public async Task JPath_PathNotPresent_PathError()
	{
		var expression = new ExtendedExpression("jPath(source, 'size')");
		expression.Parameters["source"] = JObject.Parse("{ \"name\": \"bob\", \"numbers\": [1, 2] }");
		var exception = await Assert.ThrowsAsync<NCalcExtensionsException>(expression.EvaluateAsync);
		exception.Message.Should().Be(
			"jPath function - jPath expression did not result in a match.",
			because: "the selected item is an array"
		);
	}

	[Fact]
	public async Task JPath_PathNotPresent_PathError_ReturnsNull()
	{
		var expression = new ExtendedExpression("jPath(source, 'size', True)");
		expression.Parameters["source"] = JObject.Parse("{ \"name\": \"bob\", \"numbers\": [1, 2] }");
		var result = await expression.EvaluateAsync();
		result.Should().BeNull(
			because: "the requested jPath is not present and the third parameter 'returnNullIfNotFound' is set to True"
		);
	}

	[Fact]
	public async Task JPath_BadExpression_RequiresSingleValueError()
	{
		var expression = new ExtendedExpression("jPath(source, 'numbers')");
		expression.Parameters["source"] = JObject.Parse("{ \"name\": \"bob\", \"numbers\": [1, 2] }");
		var result = await expression.EvaluateAsync();
		result.Should().BeOfType<JArray>();
		var theArray = result as JArray;
		theArray.Should().NotBeNull();
		theArray.Should().HaveCount(2);
	}
}
