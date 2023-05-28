namespace PanoramicData.NCalcAsyncExtensions.Test;

public class IsNullTests
{
	[Fact]
	public async Task IsNull_Example1_Succeeds()
	{
		var expression = new ExtendedExpression("isNull(1)");
		Assert.False(await expression.EvaluateAsync() as bool?);
	}

	[Fact]
	public async Task IsNull_Example2_Succeeds()
	{
		var expression = new ExtendedExpression("isNull('text')");
		Assert.False(await expression.EvaluateAsync() as bool?);
	}

	[Fact]
	public async Task IsNull_Example3_Succeeds()
	{
		var expression = new ExtendedExpression("isNull(bob)");
		expression.Parameters["bob"] = null;
		Assert.True(await expression.EvaluateAsync() as bool?);
	}

	[Fact]
	public async Task IsNull_Example4_Succeeds()
	{
		var expression = new ExtendedExpression("isNull(null)");
		Assert.True(await expression.EvaluateAsync() as bool?);
	}

	[Fact]
	public async Task IsNull_JObjectWithJTokenTypeOfNull_ReturnsTrue()
	{
		var theObject = new Exception(null);
		var jObject = JObject.FromObject(theObject);
		var expression = new ExtendedExpression($"isNull({nameof(jObject)})");
		expression.Parameters.Add(nameof(jObject), jObject["Message"]);
		Assert.True(await expression.EvaluateAsync() as bool?);
	}

	[Fact]
	public async Task IsNull_JObjectWithJTokenTypeOfString_ReturnsFalse()
	{
		var theObject = new Exception("A message");
		var jObject = JObject.FromObject(theObject);
		var expression = new ExtendedExpression($"isNull({nameof(jObject)})");
		expression.Parameters.Add(nameof(jObject), jObject["Message"]);
		Assert.False(await expression.EvaluateAsync() as bool?);
	}
}
