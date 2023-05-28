using System.Threading.Tasks;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class TypeOf
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		var parameter1 = functionArgs.Parameters.Length == 1
			? await functionArgs.Parameters[0].EvaluateAsync()
			: throw new FormatException($"{ExtensionFunction.TypeOf} function -  requires one parameter.");

		functionArgs.Result = parameter1 switch
		{
			null => null,
			var @object => @object.GetType().Name
		};
	}
}
