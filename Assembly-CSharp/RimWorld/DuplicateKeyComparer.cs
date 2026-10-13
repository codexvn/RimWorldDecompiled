using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x0200388C RID: 14476
	public class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable
	{
		// Token: 0x0601401D RID: 81949 RVA: 0x005F0E90 File Offset: 0x005EF090
		public int Compare(TKey x, TKey y)
		{
			int num = x.CompareTo(y);
			if (num == 0)
			{
				return -1;
			}
			return num;
		}
	}
}
