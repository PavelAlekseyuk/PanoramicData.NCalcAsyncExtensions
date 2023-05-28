using Newtonsoft.Json;

namespace PanoramicData.NCalcAsyncExtensions.Test;

public class ParseTests
{
	[Theory]
	[InlineData("")]
	[InlineData("'int', 1")]
	[InlineData("'xxx', '1'")]
	[InlineData("1")]
	public async Task Parse_IncorrectParameterCountOrType_Throws(string parameters)
	{
		var expression = new ExtendedExpression($"parse({parameters})");
		await Assert.ThrowsAsync<FormatException>(expression.EvaluateAsync);
	}

	[Theory]
	[InlineData("short")]
	[InlineData("ushort")]
	[InlineData("long")]
	[InlineData("ulong")]
	[InlineData("int")]
	[InlineData("uint")]
	[InlineData("byte")]
	[InlineData("sbyte")]
	[InlineData("double")]
	[InlineData("float")]
	[InlineData("decimal")]
	[InlineData("jObject")]
	[InlineData("jArray")]
	public async Task Parse_Unparsable_Throws(string parameters)
	{
		var expression = new ExtendedExpression($"parse('{parameters}', 'x')");
		await Assert.ThrowsAsync<FormatException>(expression.EvaluateAsync);
	}

	[Theory]
	[InlineData("true")]
	[InlineData("True")]
	[InlineData("false")]
	[InlineData("False")]
	public async Task Parse_Bool_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"parse('bool', '{text}')");
		var result = await expression.EvaluateAsync();
		result.Should().Be(bool.Parse(text));
	}

	[Theory]
	[InlineData("ttrue")]
	[InlineData("Truex")]
	[InlineData("x")]
	[InlineData("")]
	public async Task Parse_Bool_InvalidInput_Succeeds(string? text)
	{
		var expression = new ExtendedExpression($"parse('bool', a, null)");
		expression.Parameters["a"] = text;
		var result = await expression.EvaluateAsync();
		result.Should().BeNull();
	}

	[Theory]
	[InlineData("1")]
	[InlineData("-1")]
	[InlineData("-0")]
	[InlineData("0")]
	public async Task Parse_Int_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"parse('int', '{text}')");
		var result = await expression.EvaluateAsync();
		result.Should().Be(int.Parse(text));
	}

	[Theory]
	[InlineData("1")]
	[InlineData("-1")]
	[InlineData("-0")]
	[InlineData("0")]
	public async Task Parse_Long_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"parse('long', '{text}')");
		var result = await expression.EvaluateAsync();
		result.Should().Be(long.Parse(text));
	}

	[Theory]
	[InlineData("1")]
	[InlineData("0")]
	public async Task Parse_ULong_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"parse('ulong', '{text}')");
		var result = await expression.EvaluateAsync();
		result.Should().Be(ulong.Parse(text));
	}

	[Theory]
	[InlineData("1")]
	[InlineData("0")]
	public async Task Parse_UInt_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"parse('uint', '{text}')");
		var result = await expression.EvaluateAsync();
		result.Should().Be(uint.Parse(text));
	}

	[Theory]
	[InlineData("1")]
	[InlineData("-1")]
	[InlineData("-0")]
	[InlineData("0")]
	public async Task Parse_Double_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"parse('double', '{text}')");
		var result = await expression.EvaluateAsync();
		result.Should().Be(double.Parse(text));
	}

	[Theory]
	[InlineData("1")]
	[InlineData("-1")]
	[InlineData("-0")]
	[InlineData("0")]
	public async Task Parse_Float_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"parse('float', '{text}')");
		var result = await expression.EvaluateAsync();
		result.Should().Be(float.Parse(text));
	}

	[Theory]
	[InlineData("1")]
	[InlineData("-1")]
	[InlineData("-0")]
	[InlineData("0")]
	public async Task Parse_Decimal_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"parse('decimal', '{text}')");
		var result = await expression.EvaluateAsync();
		result.Should().Be(decimal.Parse(text));
	}

	[Theory]
	[InlineData("1")]
	[InlineData("-1")]
	[InlineData("-0")]
	[InlineData("0")]
	public async Task Parse_SByte_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"parse('sbyte', '{text}')");
		var result = await expression.EvaluateAsync();
		result.Should().Be(sbyte.Parse(text));
	}

	[Theory]
	[InlineData("1")]
	[InlineData("0")]
	public async Task Parse_Byte_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"parse('byte', '{text}')");
		var result = await expression.EvaluateAsync();
		result.Should().Be(byte.Parse(text));
	}

	[Theory]
	[InlineData("1")]
	[InlineData("-1")]
	[InlineData("-0")]
	[InlineData("0")]
	public async Task Parse_Short_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"parse('short', '{text}')");
		var result = await expression.EvaluateAsync();
		result.Should().Be(short.Parse(text));
	}

	[Theory]
	[InlineData("1")]
	[InlineData("0")]
	public async Task Parse_Ushort_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"parse('ushort', '{text}')");
		var result = await expression.EvaluateAsync();
		result.Should().Be(ushort.Parse(text));
	}

	[Theory]
	[InlineData("{}")]
	[InlineData("{\"a\":1}")]
	public async Task Parse_JObject_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"parse('jObject', '{text}')");
		var result = await expression.EvaluateAsync();
		JsonConvert.SerializeObject(result).Should().Be(text);
	}

	[Theory]
	[InlineData("[]")]
	[InlineData("[\"a\",1]")]
	public async Task Parse_JArray_Succeeds(string text)
	{
		var expression = new ExtendedExpression($"parse('jArray', '{text}')");
		var result = await expression.EvaluateAsync();
		JsonConvert.SerializeObject(result).Should().Be(text);
	}
}
