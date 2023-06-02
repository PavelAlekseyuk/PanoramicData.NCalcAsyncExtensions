using NCalcAsyncExtensions.Helpers;

namespace NCalcAsyncExtensions.Extensions;

internal static class Try
{
	internal static async Task EvaluateAsync(FunctionArgs functionArgs)
	{
		ExtendedExpression.CheckParameterCount(nameof(Try), functionArgs, 1, 2);

		try
		{
			functionArgs.Result = await functionArgs.Parameters[0].EvaluateSafelyAsync();
		}
		catch (Exception e)
		{
			if (functionArgs.Parameters.Length >= 2)
			{
				functionArgs.Parameters[1].Parameters["exception"] = e;
				functionArgs.Parameters[1].Parameters["exception_message"] = e.Message;
				functionArgs.Parameters[1].Parameters["exception_typeName"] = e.GetType().Name;
				functionArgs.Parameters[1].Parameters["exception_typeFullName"] = e.GetType().FullName;
				functionArgs.Parameters[1].Parameters["exception_type"] = e.GetType();
				functionArgs.Result = await functionArgs.Parameters[1].EvaluateSafelyAsync();
			}
			else
			{
				functionArgs.Result = null;
			}
		}
	}
}
