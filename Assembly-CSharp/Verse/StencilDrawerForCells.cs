using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x02000803 RID: 2051
	public class StencilDrawerForCells : IExposable
	{
		// Token: 0x0600344E RID: 13390 RVA: 0x00113270 File Offset: 0x00111470
		public void Draw()
		{
			if (this.cells.NullOrEmpty<IntVec3>())
			{
				GenDraw.DrawStencilCell(this.center, GenDraw.RitualStencilMat, (float)this.dimensionsIfNoCells.x, (float)this.dimensionsIfNoCells.z);
				return;
			}
			foreach (IntVec3 intVec in this.cells)
			{
				GenDraw.DrawStencilCell(intVec.ToVector3Shifted(), GenDraw.RitualStencilMat, 1f, 1f);
			}
		}

		// Token: 0x0600344F RID: 13391 RVA: 0x00113310 File Offset: 0x00111510
		public void ExposeData()
		{
			Scribe_References.Look<Lord>(ref this.sourceLord, "sourceLord", false);
			Scribe_Collections.Look<IntVec3>(ref this.cells, "cells", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<Vector3>(ref this.center, "center", default(Vector3), false);
			Scribe_Values.Look<IntVec2>(ref this.dimensionsIfNoCells, "dimensionsIfNoCells", default(IntVec2), false);
			Scribe_Values.Look<int>(ref this.ticksLeftWithoutLord, "ticksLeftWithoutLord", 0, false);
		}

		// Token: 0x04002778 RID: 10104
		public Lord sourceLord;

		// Token: 0x04002779 RID: 10105
		public List<IntVec3> cells;

		// Token: 0x0400277A RID: 10106
		public Vector3 center;

		// Token: 0x0400277B RID: 10107
		public IntVec2 dimensionsIfNoCells;

		// Token: 0x0400277C RID: 10108
		public int ticksLeftWithoutLord;
	}
}
