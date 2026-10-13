using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200174B RID: 5963
	public class LordToilData_Speech : LordToilData_Gathering
	{
		// Token: 0x06008C0C RID: 35852 RVA: 0x0029B224 File Offset: 0x00299424
		public override void ExposeData()
		{
			Scribe_Values.Look<CellRect>(ref this.spectateRect, "spectateRect", default(CellRect), false);
			Scribe_Values.Look<SpectateRectSide>(ref this.spectateRectAllowedSides, "spectateRectAllowedSides", SpectateRectSide.None, false);
			Scribe_Values.Look<SpectateRectSide>(ref this.spectateRectPreferredSide, "spectateRectPreferredSide", SpectateRectSide.None, false);
		}

		// Token: 0x04005B04 RID: 23300
		public CellRect spectateRect;

		// Token: 0x04005B05 RID: 23301
		public SpectateRectSide spectateRectAllowedSides = SpectateRectSide.All;

		// Token: 0x04005B06 RID: 23302
		public SpectateRectSide spectateRectPreferredSide;
	}
}
