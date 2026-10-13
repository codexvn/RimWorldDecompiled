using System;

namespace Verse
{
	// Token: 0x0200016E RID: 366
	public class PawnStagePosition : IExposable
	{
		// Token: 0x06000AA3 RID: 2723 RVA: 0x000321CE File Offset: 0x000303CE
		public PawnStagePosition()
		{
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x000321E1 File Offset: 0x000303E1
		public PawnStagePosition(IntVec3 cell, Thing thing, Rot4 orientation, bool highlight)
		{
			this.cell = cell;
			this.thing = thing;
			this.orientation = orientation;
			this.highlight = highlight;
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x00032214 File Offset: 0x00030414
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.cell, "cell", default(IntVec3), false);
			Scribe_Values.Look<Rot4>(ref this.orientation, "orientation", Rot4.Invalid, false);
			Scribe_Values.Look<bool>(ref this.highlight, "highlight", false, false);
			Scribe_References.Look<Thing>(ref this.thing, "thing", false);
		}

		// Token: 0x04000596 RID: 1430
		public IntVec3 cell;

		// Token: 0x04000597 RID: 1431
		public Thing thing;

		// Token: 0x04000598 RID: 1432
		public Rot4 orientation = Rot4.Invalid;

		// Token: 0x04000599 RID: 1433
		public bool highlight;
	}
}
