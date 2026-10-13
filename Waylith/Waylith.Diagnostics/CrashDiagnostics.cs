using System;
using System.Collections.Generic;
using System.Threading;

namespace Waylith.Diagnostics;

public static class CrashDiagnostics
{
	private static readonly List<Func<string?>> Providers = new List<Func<string>>();

	private static readonly Lock Sync = new Lock();

	public static void Register(Func<string?> provider)
	{
		using (Sync.EnterScope())
		{
			Providers.Add(provider);
		}
	}

	internal static Func<string?>[] Snapshot()
	{
		using (Sync.EnterScope())
		{
			return Providers.ToArray();
		}
	}
}
