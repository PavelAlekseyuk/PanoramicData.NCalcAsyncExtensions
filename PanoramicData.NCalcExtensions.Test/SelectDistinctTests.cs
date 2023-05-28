using System.Collections.Generic;

namespace PanoramicData.NCalcAsyncExtensions.Test;

public class SelectDistinctTests : NCalcTest
{
	[Fact]
	public async Task SelectDistinct_Succeeds()
	{
		var result = await new ExtendedExpression($"selectDistinct(list(1, 2, 3, 3, 3), 'n', 'n + 1')").EvaluateAsync();

		// The result should be 4, 5, 6
		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(new List<int> { 2, 3, 4 });
	}
}