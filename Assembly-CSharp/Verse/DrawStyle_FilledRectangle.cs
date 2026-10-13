using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020003EA RID: 1002
	public class DrawStyle_FilledRectangle : DrawStyle
	{
		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x060019A5 RID: 6565 RVA: 0x00002501 File Offset: 0x00000701
		public override bool CanHaveDuplicates
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060019A6 RID: 6566 RVA: 0x000816EC File Offset: 0x0007F8EC
		public override void Update(IntVec3 origin, IntVec3 target, List<IntVec3> buffer)
		{
			buffer.AddRange(CellRect.FromLimits(origin, target).Cells);
		}
	}
}
