using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000918 RID: 2328
	public abstract class Designator_Cells : Designator
	{
		// Token: 0x06003C6C RID: 15468 RVA: 0x0013CBB3 File Offset: 0x0013ADB3
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}
	}
}
