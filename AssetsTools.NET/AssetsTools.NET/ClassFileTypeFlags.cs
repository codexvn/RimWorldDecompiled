using System;

namespace AssetsTools.NET;

[Flags]
public enum ClassFileTypeFlags : byte
{
	None = 0,
	IsAbstract = 1,
	IsSealed = 2,
	IsEditorOnly = 4,
	IsReleaseOnly = 8,
	IsStripped = 0x10,
	Reserved = 0x20,
	HasEditorRootNode = 0x40,
	HasReleaseRootNode = 0x80
}
