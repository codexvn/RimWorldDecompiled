using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000818 RID: 2072
	public class FastVector2Comparer : IEqualityComparer<Vector2>
	{
		// Token: 0x060034B4 RID: 13492 RVA: 0x00114EB1 File Offset: 0x001130B1
		public bool Equals(Vector2 x, Vector2 y)
		{
			return x == y;
		}

		// Token: 0x060034B5 RID: 13493 RVA: 0x00114EBA File Offset: 0x001130BA
		public int GetHashCode(Vector2 obj)
		{
			return obj.GetHashCode();
		}

		// Token: 0x040027B3 RID: 10163
		public static readonly FastVector2Comparer Instance = new FastVector2Comparer();
	}
}
