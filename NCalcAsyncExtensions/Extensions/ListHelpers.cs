﻿using System.Collections.Generic;

namespace NCalcAsyncExtensions.Extensions;

internal static class ListHelpers
{
	internal static List<object?> Collapse(this List<object?> value)
	{
		while (value.All(v => v?.GetType() == typeof(List<object?>)))
		{
			if (value.Count == 0)
			{
				return new List<object?>();
			}

			value = value.SelectMany(v => v as List<object?>).ToList();
		}

		return value;
	}
}
