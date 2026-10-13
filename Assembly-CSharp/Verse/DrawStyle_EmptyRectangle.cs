using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020003E8 RID: 1000
	public class DrawStyle_EmptyRectangle : DrawStyle
	{
		// Token: 0x0600199D RID: 6557 RVA: 0x00081594 File Offset: 0x0007F794
		public override void Update(IntVec3 origin, IntVec3 target, List<IntVec3> buffer)
		{
			buffer.AddRange(CellRect.FromLimits(origin, target).EdgeCells);
		}
	}
}
