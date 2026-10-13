using System;

namespace AssetsTools.NET;

[Flags]
public enum TypeTreeNodeFlags
{
	None = 0,
	Array = 1,
	Ref = 2,
	Registry = 4,
	ArrayOfRefs = 8
}
