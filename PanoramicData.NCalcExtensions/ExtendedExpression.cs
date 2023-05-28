using PanoramicData.NCalcAsyncExtensions.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PanoramicData.NCalcAsyncExtensions;

public class ExtendedExpression : Expression
{
	private static readonly Dictionary<string, object?> _storageDictionary = new();
	internal const string StorageDictionaryParameterName = "__storageDictionary";

	public ExtendedExpression(string expression) : base(expression)
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

	internal static async Task ExtendAsync(string functionName, FunctionArgs functionArgs)
	{
		if (functionArgs == null)
		{
			throw new ArgumentNullException(nameof(functionArgs));
		}

		switch (functionName)
		{
			case ExtensionFunction.All:
				await All.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.Any:
				await Any.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.CanEvaluate:
				await CanEvaluate.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.Capitalise:
			case ExtensionFunction.Capitalize:
				await Capitalize.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.Cast:
				await Cast.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.ChangeTimeZone:
				await ChangeTimeZone.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.Concat:
				await Concat.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.Contains:
				await Contains.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.Convert:
				await ConvertFunction.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.Count:
				await Count.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.DateTime:
				await DateTimeMethods.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.DateTimeAsEpochMs:
				await DateTimeAsEpochMs.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.Distinct:
				await Distinct.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.EndsWith:
				await EndsWith.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.Format:
				await Format.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.GetProperty:
				await GetProperty.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.Humanise:
			case ExtensionFunction.Humanize:
				await Humanize.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.In:
				await In.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.IndexOf:
				await IndexOf.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.If:
				await If.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.IsInfinite:
				await IsInfinite.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.IsNaN:
				await IsNaN.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.IsNull:
				await IsNull.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.IsNullOrEmpty:
				await IsNullOrEmpty.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.IsNullOrWhiteSpace:
				await IsNullOrWhiteSpace.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.IsSet:
				await IsSet.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.ItemAtIndex:
				await ItemAtIndex.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.Join:
				await Join.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.JPath:
				await JPath.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.LastIndexOf:
				await LastIndexOf.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.Length:
				await Length.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.List:
				await List.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.ListOf:
				await ListOf.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.Max:
				await Max.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.Min:
				await Min.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.NullCoalesce:
				await NullCoalesce.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.NewJObject:
				await NewJObject.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.OrderBy:
				await OrderBy.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.PadLeft:
				await PadLeft.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.Parse:
				await Parse.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.ParseInt:
				await ParseInt.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.RegexGroup:
				await RegexGroup.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.RegexIsMatch:
				await RegexIsMatch.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.Replace:
				await Replace.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.Retrieve:
				await Retrieve.EvaluateAsync(functionArgs, _storageDictionary);
				return;
			case ExtensionFunction.Select:
				await Select.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.SelectDistinct:
				await SelectDistinct.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.SetProperties:
				await SetProperties.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.Skip:
				await Skip.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.Sort:
				await Sort.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.Split:
				await Split.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.StartsWith:
				await StartsWith.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.Store:
				await Store.EvaluateAsync(functionArgs, _storageDictionary);
				return;
			case ExtensionFunction.Substring:
				await Substring.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.Sum:
				await Sum.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.Switch:
				await Switch.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.Take:
				await Take.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.Throw:
				throw await Throw.EvaluateAsync(functionArgs);
			case ExtensionFunction.TimeSpan:
			case ExtensionFunction.TimeSpanCamel:
				await Extensions.TimeSpan.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.ToDateTime:
				await ToDateTime.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.ToLower:
				await ToLower.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.ToString:
				await Extensions.ToString.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.ToUpper:
				await ToUpper.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.Try:
				await Try.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.TypeOf:
				await TypeOf.EvaluateAsync(functionArgs);
				return;
			case ExtensionFunction.Where:
				await Where.EvaluateAsync(functionArgs);
				return;
			default:
				return;
		}
	}
}
