﻿namespace PanoramicData.NCalcAsyncExtensions.Test;

public class OrderByTests : NCalcTest
{
	[Theory]
	[InlineData("n", new[] { 1, 2, 3 })]
	[InlineData("-n", new[] { 3, 2, 1 })]
	public void OrderBy_SingleTerm_Succeeds(string expression, int[] expectedOrder)
		=> new ExtendedExpression($"orderBy(list(2, 1, 3), 'n', '{expression}')")
		.Evaluate()
		.Should()
		.BeEquivalentTo(expectedOrder);

	[Theory]
	[InlineData("n % 32", "n % 2", new[] { 34, 33, 1, 2 })]
	[InlineData("n % 2", "n % 32", new[] { 33, 1, 34, 2 })]
	public void OrderBy_MultipleTerms_Succeeds(string expression1, string expression2, int[] expectedOrder)
	=> new ExtendedExpression($"orderBy(list(34, 33, 2, 1), 'n', '{expression1}', '{expression2}')")
	.Evaluate()
	.Should()
	.BeEquivalentTo(expectedOrder);
}
