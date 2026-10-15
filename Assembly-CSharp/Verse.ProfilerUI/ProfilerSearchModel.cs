using System;
using System.Collections.Generic;
using Waylith.Profiling;

namespace Verse.ProfilerUI;

public sealed class ProfilerSearchModel
{
	private readonly Dictionary<ProfilerLabelId, bool> matchCache = new Dictionary<ProfilerLabelId, bool>();

	private string query = "";

	public string Query
	{
		get
		{
			return query;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			if (!(query == value))
			{
				query = value;
				matchCache.Clear();
			}
		}
	}

	public bool IsActive => Query.Length > 0;

	public bool Matches(ProfilerLabelId labelId)
	{
		if (!IsActive)
		{
			return true;
		}
		if (!labelId.IsValid)
		{
			return false;
		}
		if (!matchCache.TryGetValue(labelId, out var value))
		{
			value = FrameProfiler.GetLabel(labelId).Contains(Query, StringComparison.OrdinalIgnoreCase);
			matchCache.Add(labelId, value);
		}
		return value;
	}

	public bool Matches(string label)
	{
		if (IsActive)
		{
			return label?.Contains(Query, StringComparison.OrdinalIgnoreCase) ?? false;
		}
		return true;
	}
}
