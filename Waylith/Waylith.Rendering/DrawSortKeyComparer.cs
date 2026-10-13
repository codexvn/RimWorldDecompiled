using System.Collections.Generic;

namespace Waylith.Rendering;

public sealed class DrawSortKeyComparer : IComparer<FinalDrawItem>
{
	public static readonly DrawSortKeyComparer Instance = new DrawSortKeyComparer();

	public int Compare(FinalDrawItem? a, FinalDrawItem? b)
	{
		return a.SortKey.CompareTo(b.SortKey);
	}
}
