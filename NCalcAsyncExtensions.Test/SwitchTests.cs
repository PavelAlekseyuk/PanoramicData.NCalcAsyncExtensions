namespace NCalcAsyncExtensions.Test;

public class SwitchTests : NCalcTest
{
	[Theory]
	[InlineData("switch()")]
	[InlineData("switch(1)")]
	[InlineData("switch(1, 2)")]
	public async Task Switch_InsufficientParameters_ThrowsException(string expression)
		=> await Assert.ThrowsAsync<FormatException>(() => new ExtendedExpression(expression).EvaluateAsync());

	[Theory]
	[InlineData("switch('yes', 'yes', 1)", 1)]
	[InlineData("switch('yes', 'yes', 1, 'no', 2)", 1)]
	[InlineData("switch('yes', 'yes', '1', 'no', '2')", "1")]
	[InlineData("switch('no', 'yes', 1, 'no', 2)", 2)]
	[InlineData("switch('no', 'yes', '1', 'no', '2')", "2")]
	[InlineData("switch('blah', 'yes', 1, 'no', 2, 3)", 3)]
	[InlineData("switch('blah', 'yes', 1, 'no', 2, '3')", "3")]
	[InlineData("switch(1, 1, 'one', 2, 'two')", "one")]
	public async Task Switch_ReturnsExpected(string expression, object expectedOutput)
		=> Assert.Equal(expectedOutput, await new ExtendedExpression(expression).EvaluateAsync());

	[Theory]
	[InlineData("switch('blah', 'yes', 1, 'no', 2)")]
	public async Task Switch_MissingDefault_ThrowsException(string expression)
		=> await Assert.ThrowsAsync<FormatException>(() => new ExtendedExpression(expression).EvaluateAsync());

	[Fact]
	public async Task Switch_ComparingIntegers_Works()
	{
		const string expression = "switch(incident_Priority, 4, 4, 1, 1, 21)";
		var e = new ExtendedExpression(expression);
		e.Parameters["incident_Priority"] = 1;
		var result = await e.EvaluateAsync();
		result.Should().Be(1);
	}

	[Fact]
	public async Task Switch_ComparingIntegersInsideIf_Works()
	{
		const string expression = "if(incident_exists, switch(incident_Priority, 4, 4, 1, 1, 21), 9)";
		var e = new ExtendedExpression(expression);
		e.Parameters["incident_exists"] = true;
		e.Parameters["incident_Priority"] = 4;
		var result = await e.EvaluateAsync();
		result.Should().Be(4);
	}

	[Fact]
	public async Task Switch_ComparingIntegersViaJObject_Works()
	{
		const string expression = "if(incident_exists, switch(incident_Priority, 4, 4, 1, 1, 21), 9)";
		var e = new ExtendedExpression(expression);
		var jobject = new JObject
		{
			["incident_exists"] = true,
			["incident_Priority"] = 4
		};

		foreach (var property in jobject.Properties())
		{
			e.Parameters[property.Name] = GetValue(property);
		}

		var result = await e.EvaluateAsync();
		result.Should().Be(4);
	}

	private static object? GetValue(JProperty jProperty) => jProperty.Value.Type switch
	{
		JTokenType.Null => (object?)null,
		JTokenType.Undefined => (object?)null,
		JTokenType.String => jProperty.Value.ToObject<string>(),
		JTokenType.Integer => jProperty.Value.ToObject<int>(),
		JTokenType.Float => jProperty.Value.ToObject<double>(),
		JTokenType.Boolean => jProperty.Value.ToObject<bool>(),
		_ => jProperty.Value
	};

}

