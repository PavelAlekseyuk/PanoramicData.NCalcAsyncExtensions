using System.Collections.Generic;

namespace NCalcAsyncExtensions.Test;

public class SelectTests : NCalcTest
{
	[Fact]
	public async Task Select_Succeeds()
	{
		var result = await new ExtendedExpression($"select(list(1, 2, 3, 4, 5), 'n', 'n + 1')").EvaluateAsync();

		// The result should be 2, 3, 4, 5, 6
		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(new List<int> { 2, 3, 4, 5, 6 });
	}
}
