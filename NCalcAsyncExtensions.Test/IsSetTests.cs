namespace NCalcAsyncExtensions.Test;

public class IsSetTests
{
	[Fact]
	public async Task IsSet_IsNotSet_ReturnsFalse()
	{
		var expression = new ExtendedExpression("isSet('a')");
		Assert.False(await expression.EvaluateAsync() as bool?);
	}

	[Fact]
	public async Task IsSet_IsNotSetWithParameterReferenceNotSet_ReturnsTrue()
	{
		var expression = new ExtendedExpression("isSet('a') && !isNull(a) && a!=''");
		Assert.False(await expression.EvaluateAsync() as bool?);
	}

	[Fact]
	public async Task IsSet_IsNotSetWithParameterReferenceSet_ReturnsTrue()
	{
		var expression = new ExtendedExpression("isSet('a.b') && !isNull([a.b]) && [a.b]!=''");
		expression.Parameters["a.b"] = 1;
		Assert.True(await expression.EvaluateAsync() as bool?);
	}

	[Fact]
	public async Task IsSet_IsSet_ReturnsTrue()
	{
		var expression = new ExtendedExpression("isSet('a')");
		expression.Parameters["a"] = 1;
		Assert.True(await expression.EvaluateAsync() as bool?);
	}

	[Fact]
	public async Task IsSet_IsSetToNull_ReturnsTrue()
	{
		var expression = new ExtendedExpression("isSet('a')");
		expression.Parameters["a"] = null;
		Assert.True(await expression.EvaluateAsync() as bool?);
	}
}
