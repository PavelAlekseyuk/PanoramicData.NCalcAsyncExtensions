using NCalcAsyncExtensions.Extensions;
using System.Collections.Generic;

namespace NCalcAsyncExtensions;

public class ExtendedExpression : Expression
{
	private static readonly Dictionary<string, object?> _storageDictionary = new();
	internal const string StorageDictionaryParameterName = "__storageDictionary";

	public ExtendedExpression(string expression) 
		: base(expression)
	{
		Parameters[StorageDictionaryParameterName] = _storageDictionary;
		EvaluateFunctionAsync += ExtendAsync;
		CacheEnabled = false;
		if (Parameters.ContainsKey("null"))
		{
			throw new InvalidOperationException("You may not set a parameter called 'null', as it is a reserved keyword.");
		}

		Parameters["null"] = null;
		if (Parameters.ContainsKey("True"))
		{
			throw new InvalidOperationException("You may not set a parameter called 'True', as it is a reserved keyword.");
		}

		Parameters["True"] = true;
		if (Parameters.ContainsKey("False"))
		{
			throw new InvalidOperationException("You may not set a parameter called 'False', as it is a reserved keyword.");
		}

		Parameters["False"] = false;
	}

	internal static void CheckParameterCount(
		string functionName,
		FunctionArgs functionArgs,
		int? minPropertyCount,
		int? maxPropertyCount)
	{
		if (minPropertyCount is not null && functionArgs.Parameters.Length < minPropertyCount)
		{
			throw new FormatException($"{functionName}: At least {minPropertyCount} parameter{(minPropertyCount == 1 ? "" : "s")} required.");
		}

		if (maxPropertyCount is not null && functionArgs.Parameters.Length > maxPropertyCount)
		{
			throw new FormatException($"{functionName}: No more than {maxPropertyCount} parameter{(maxPropertyCount == 1 ? "" : "s")} permitted.");
		}
	}
}
