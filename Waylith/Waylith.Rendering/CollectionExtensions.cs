using System;
using System.Collections.Generic;

namespace Waylith.Rendering;

public static class CollectionExtensions
{
	public static void SortBy<T, TSortBy>(this List<T> list, Func<T, TSortBy> selector) where TSortBy : IComparable<TSortBy>
	{
		list.Sort((T a, T b) => selector(a).CompareTo(selector(b)));
	}
}
