﻿namespace PanoramicData.NCalcAsyncExtensions.Extensions;

internal static class ToString
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		var parameterCount = functionArgs.Parameters.Length;
		switch (parameterCount)
		{
			case 1:
				var parameter1 = functionArgs.Parameters[0].Evaluate();
				functionArgs.Result = parameter1 switch
				{
					null => null,
					object @object => @object.ToString()
				};
				break;
			case 2:
				var parameter1a = functionArgs.Parameters[0].Evaluate();
				var parameter2 = functionArgs.Parameters[1].Evaluate() as string
					?? throw new FormatException($"{ExtensionFunction.ToString} function -  requires a string as the second parameter.");
				functionArgs.Result = parameter1a switch
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
					object @object => @object.ToString()
				};
				break;
			default:
				throw new FormatException($"{ExtensionFunction.ToString} function -  requires one or two parameters.");
		}
	}
}
