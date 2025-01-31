﻿namespace NCalcAsyncExtensions.Test;

public class SetPropertiesTests
{
	[Fact]
	public async Task SetProperties_OnJObject_CreatesJObject()
	{
		var expression = new ExtendedExpression("setProperties(jObject('a', 1, 'b', null), 'c', 'X')");
		var result = await expression.EvaluateAsync() as JObject;
		result.Should().BeOfType<JObject>();
		result.Should().NotBeNull();
		result.Should().HaveCount(3);
		result!["a"].Should().BeOfType<JValue>();
		result["a"].Should().BeEquivalentTo(JToken.FromObject(1));
		result["b"].Should().BeOfType<JValue>();
		result["b"].Should().BeEquivalentTo(JValue.CreateNull());
		result["c"].Should().BeEquivalentTo(JToken.FromObject("X"));
	}

	[Fact]
	public async Task SetProperties_OnAnonymous_CreatesJObject()
	{
		var expression = new ExtendedExpression("setProperties(anon, 'c', 'X')");
		expression.Parameters["anon"] = new { a = 1, b = (string?)null };
		var result = await expression.EvaluateAsync() as JObject;
		result.Should().BeOfType<JObject>();
		result.Should().NotBeNull();
		result.Should().HaveCount(3);
		result!["a"].Should().BeOfType<JValue>();
		result["a"].Should().BeEquivalentTo(JToken.FromObject(1));
		result["b"].Should().BeOfType<JValue>();
		result["b"].Should().BeEquivalentTo(JValue.CreateNull());
		result["c"].Should().BeEquivalentTo(JToken.FromObject("X"));
	}
}
