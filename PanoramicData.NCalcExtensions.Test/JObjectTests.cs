namespace PanoramicData.NCalcAsyncExtensions.Test;

public class JObjectTests
{
	[Fact]
	public async Task JObject_CreatesJObject()
	{
		var expression = new ExtendedExpression("jObject('a', 1, 'b', null)");
		var result = await expression.EvaluateAsync() as JObject;
		result.Should().BeOfType<JObject>();
		result.Should().NotBeNull();
		result.Should().HaveCount(2);
		result["a"].Should().BeOfType<JValue>();
		result["a"].Should().BeEquivalentTo(JToken.FromObject(1));
		result["b"].Should().BeOfType<JValue>();
		result["b"].Should().BeEquivalentTo(JValue.CreateNull());
	}
}
