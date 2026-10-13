using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001748 RID: 5960
	public class LordToilData_MarriageCeremony : LordToilData
	{
		// Token: 0x06008C03 RID: 35843 RVA: 0x0029AF9C File Offset: 0x0029919C
		public override void ExposeData()
		{
			Scribe_Values.Look<CellRect>(ref this.spectateRect, "spectateRect", default(CellRect), false);
			Scribe_Values.Look<SpectateRectSide>(ref this.spectateRectAllowedSides, "spectateRectAllowedSides", SpectateRectSide.None, false);
		}

		// Token: 0x04005B00 RID: 23296
		public CellRect spectateRect;

		// Token: 0x04005B01 RID: 23297
		public SpectateRectSide spectateRectAllowedSides = SpectateRectSide.All;
	}
}
