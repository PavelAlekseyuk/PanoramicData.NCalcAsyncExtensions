namespace PanoramicData.NCalcAsyncExtensions.Test;

public class SortTests : NCalcTest
{
	[Theory]
	[InlineData(null, new[] { 1, 2, 3 })]
	[InlineData("asc", new[] { 1, 2, 3 })]
	[InlineData("desc", new[] { 3, 2, 1 })]
	public async Task Sort_Ints_Succeeds(string? direction, int[] expectedOrder) =>
		(await new ExtendedExpression($"sort(list(2, 1, 3){(direction is null ? string.Empty : $", '{direction}'")})").EvaluateAsync())
		.Should()
		.BeEquivalentTo(expectedOrder);

	[Theory]
	[InlineData(null, new[] { "1", "2", "3" })]
	[InlineData("asc", new[] { "1", "2", "3" })]
	[InlineData("desc", new[] { "3", "2", "1" })]
	public async Task Sort_Strings_Succeeds(string? direction, string[] expectedOrder) =>
		(await new ExtendedExpression($"sort(list('2', '1', '3'){(direction is null ? string.Empty : $", '{direction}'")})").EvaluateAsync())
		.Should()
		.BeEquivalentTo(expectedOrder);
}
