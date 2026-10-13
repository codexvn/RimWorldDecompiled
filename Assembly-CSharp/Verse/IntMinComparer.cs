using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020005C0 RID: 1472
	public struct IntMinComparer : IComparer<int>
	{
		// Token: 0x060025E7 RID: 9703 RVA: 0x000C744E File Offset: 0x000C564E
		public int Compare(int x, int y)
		{
			if (x < y)
			{
				return -1;
			}
			if (x > y)
			{
				return 1;
			}
			return 0;
		}
	}
}
