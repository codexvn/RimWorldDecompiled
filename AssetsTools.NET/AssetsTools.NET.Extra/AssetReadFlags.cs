using System;

namespace AssetsTools.NET.Extra;

[Flags]
public enum AssetReadFlags
{
	None = 0,
	PreferEditor = 1,
	SkipMonoBehaviourFields = 2,
	ForceFromCldb = 4
}
