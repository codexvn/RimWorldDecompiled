using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020005BF RID: 1471
	public struct FloatMinComparer : IComparer<float>
	{
		// Token: 0x060025E6 RID: 9702 RVA: 0x000C743F File Offset: 0x000C563F
		public int Compare(float x, float y)
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
