using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace UnityStubs.Internal;

internal static class AssetRegistry
{
	private static readonly Lock Gate = new Lock();

	private static readonly List<WeakReference<UnityEngine.Object>> Live = new List<WeakReference<UnityEngine.Object>>();

	private static int _addsSinceCompact;

	public static void Register(UnityEngine.Object asset)
	{
		using (Gate.EnterScope())
		{
			Live.Add(new WeakReference<UnityEngine.Object>(asset));
			if (++_addsSinceCompact >= Math.Max(4096, Live.Count / 2))
			{
				Live.RemoveAll((WeakReference<UnityEngine.Object> weak) => !weak.TryGetTarget(out var _));
				_addsSinceCompact = 0;
			}
		}
	}

	public static void Collect(Type type, List<object> into)
	{
		using (Gate.EnterScope())
		{
			foreach (WeakReference<UnityEngine.Object> item in Live)
			{
				if (item.TryGetTarget(out var target) && target != null && type.IsInstanceOfType(target))
				{
					into.Add(target);
				}
			}
		}
	}
}
