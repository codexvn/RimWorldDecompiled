using System;

namespace Waylith.Assets;

public static class AssetBakeHooks
{
	public static event Action? BootComplete;

	public static void RaiseBootComplete()
	{
		AssetBakeHooks.BootComplete?.Invoke();
	}
}
