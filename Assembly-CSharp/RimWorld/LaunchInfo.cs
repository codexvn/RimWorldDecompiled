using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002A12 RID: 10770
	public class LaunchInfo : IExposable
	{
		// Token: 0x0600EE5A RID: 61018 RVA: 0x00452884 File Offset: 0x00450A84
		public void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.pilot, "pilot", false);
			Scribe_References.Look<Pawn>(ref this.copilot, "copilot", false);
			Scribe_Values.Look<float>(ref this.quality, "quality", 0f, false);
			Scribe_Values.Look<bool>(ref this.doNegativeOutcome, "doNegativeOutcome", false, false);
		}

		// Token: 0x04009DA0 RID: 40352
		public Pawn pilot;

		// Token: 0x04009DA1 RID: 40353
		public Pawn copilot;

		// Token: 0x04009DA2 RID: 40354
		public float quality;

		// Token: 0x04009DA3 RID: 40355
		public bool doNegativeOutcome;
	}
}
