namespace NCalcAsyncExtensions.Test;

public class ThrowTests
{
	[Fact]
	public async Task Throw_Example1_Succeeds()
	{
		var expression = new ExtendedExpression("throw()");
		await Assert.ThrowsAsync<NCalcExtensionsException>(expression.EvaluateAsync);
	}

	[Fact]
	public async Task Throw_Example2_Succeeds()
	{
		var expression = new ExtendedExpression("throw('This is a message')");
		await Assert.ThrowsAsync<NCalcExtensionsException>(expression.EvaluateAsync);
	}

	[Fact]
	public async Task Throw_Example3_Succeeds()
	{
		var expression = new ExtendedExpression("if(true, throw('There is a problem'), 5)");
		await Assert.ThrowsAsync<NCalcExtensionsException>(expression.EvaluateAsync);
	}

	[Fact]
	public async Task Throw_BadParameter_Fails()
	{
		var expression = new ExtendedExpression("throw(666)");
		await Assert.ThrowsAsync<FormatException>(expression.EvaluateAsync);
	}

	[Fact]
	public async Task Throw_TooManyParameters_Fails()
	{
		var expression = new ExtendedExpression("throw('a', 'b')");
		await Assert.ThrowsAsync<FormatException>(expression.EvaluateAsync);
	}

	[Fact]
	public async Task InnerThrow_Fails()
	{
		var expression = new ExtendedExpression("if(false, 1, throw('sdf' + a))");
		expression.Parameters["a"] = "blah";
		await Assert.ThrowsAsync<NCalcExtensionsException>(expression.EvaluateAsync);
	}
}
