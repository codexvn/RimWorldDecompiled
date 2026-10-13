using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200055A RID: 1370
	public static class ZoneMaker
	{
		// Token: 0x0600239F RID: 9119 RVA: 0x000B86E4 File Offset: 0x000B68E4
		public static Zone MakeZoneWithCells(Zone z, IEnumerable<IntVec3> cells)
		{
			if (cells != null)
			{
				foreach (IntVec3 c in cells)
				{
					z.AddCell(c);
				}
			}
			return z;
		}
	}
}
