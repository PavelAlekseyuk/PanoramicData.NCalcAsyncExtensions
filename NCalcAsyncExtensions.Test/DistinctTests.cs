using System.Collections.Generic;

namespace NCalcAsyncExtensions.Test;

public class DistinctTests : NCalcTest
{
	[Fact]
	public async Task Distinct_Succeeds()
	{
		var result = await (new ExtendedExpression($"distinct(list(1, 2, 3, 3, 3))"))
			.EvaluateAsync();

		// The result should be 1, 2, 3
		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(new List<int> { 1, 2, 3 });
	}
}
