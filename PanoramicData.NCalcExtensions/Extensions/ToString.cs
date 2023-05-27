using NCalcAsync;
using System.Threading.Tasks;

namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class ToString
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		var parameterCount = functionArgs.Parameters.Length;
		object? parameter1;
		switch (parameterCount)
		{
			case 1:
				parameter1 = await functionArgs.Parameters[0].EvaluateAsync();
				functionArgs.Result = parameter1 switch
				{
					null => null,
					var @object => @object.ToString()
				};
				break;
			case 2:
				parameter1 = await functionArgs.Parameters[0].EvaluateAsync();
				var parameter2 = await functionArgs.Parameters[1].EvaluateAsync() as string
					?? throw new FormatException($"{ExtensionFunction.ToString} function -  requires a string as the second parameter.");
				functionArgs.Result = parameter1 switch
				{
					null => null,
					byte value => value.ToString(parameter2),
					int value => value.ToString(parameter2),
					uint value => value.ToString(parameter2),
					long value => value.ToString(parameter2),
					ulong value => value.ToString(parameter2),
					short value => value.ToString(parameter2),
					ushort value => value.ToString(parameter2),
					float value => value.ToString(parameter2),
					double value => value.ToString(parameter2),
					DateTime value => value.ToString(parameter2),
					DateTimeOffset value => value.ToString(parameter2),
					var @object => @object.ToString()
				};
				break;
			default:
				throw new FormatException($"{ExtensionFunction.ToString} function -  requires one or two parameters.");
		}
	}
}
