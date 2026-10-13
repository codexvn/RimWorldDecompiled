using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020004B5 RID: 1205
	public class ScatterValidator_AvoidUsedRects : ScattererValidator
	{
		// Token: 0x06001F4E RID: 8014 RVA: 0x000A4924 File Offset: 0x000A2B24
		public override bool Allows(IntVec3 c, Map map)
		{
			List<CellRect> list;
			if (!MapGenerator.TryGetVar<List<CellRect>>("UsedRects", out list))
			{
				return true;
			}
			CellRect cellRect = new CellRect(c.x, c.z, this.size.x, this.size.z);
			foreach (CellRect other in list)
			{
				if (cellRect.Overlaps(other))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04001AFE RID: 6910
		public IntVec2 size = new IntVec2(1, 1);
	}
}
