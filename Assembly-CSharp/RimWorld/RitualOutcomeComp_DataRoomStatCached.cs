using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200273A RID: 10042
	public class RitualOutcomeComp_DataRoomStatCached : RitualOutcomeComp_Data
	{
		// Token: 0x0600DC63 RID: 56419 RVA: 0x0040E0ED File Offset: 0x0040C2ED
		public override void Reset()
		{
			base.Reset();
			this.startingVal = null;
		}

		// Token: 0x0600DC64 RID: 56420 RVA: 0x0040E104 File Offset: 0x0040C304
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float?>(ref this.startingVal, "startingVal", null, false);
		}

		// Token: 0x040094C5 RID: 38085
		public float? startingVal;
	}
}
