using System;
using System.Collections.Concurrent;
using System.Threading;
using Waylith.Collections;

namespace Waylith.Rendering;

public static class ShaderPropertyId
{
	private static readonly ConcurrentDictionary<string, int> IdsByName = new ConcurrentDictionary<string, int>(StringComparer.Ordinal);

	private static readonly ConcurrentAppendList<string> NamesById = new ConcurrentAppendList<string>();

	private static readonly Lock WriteLock = new Lock();

	public static int GetOrCreate(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return 0;
		}
		if (IdsByName.TryGetValue(name, out var value))
		{
			return value;
		}
		using (WriteLock.EnterScope())
		{
			if (IdsByName.TryGetValue(name, out value))
			{
				return value;
			}
			int num = NamesById.Add(name) + 1;
			IdsByName[name] = num;
			return num;
		}
	}

	public static bool TryGetName(int id, out string name)
	{
		if (id > 0 && NamesById.TryGet(id - 1, out name))
		{
			return true;
		}
		name = string.Empty;
		return false;
	}
}
