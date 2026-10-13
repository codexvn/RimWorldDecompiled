using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003791 RID: 14225
	public struct SkullspikeSighting : IExposable
	{
		// Token: 0x1700325B RID: 12891
		// (get) Token: 0x06013B34 RID: 80692 RVA: 0x005DA70A File Offset: 0x005D890A
		public int TicksSinceSighting
		{
			get
			{
				return Find.TickManager.TicksGame - this.tickSighted;
			}
		}

		// Token: 0x06013B35 RID: 80693 RVA: 0x005DA71D File Offset: 0x005D891D
		public SkullspikeSighting(Thing skullspike, int tickSighted)
		{
			this.skullspike = skullspike;
			this.tickSighted = tickSighted;
		}

		// Token: 0x06013B36 RID: 80694 RVA: 0x005DA72D File Offset: 0x005D892D
		public void ExposeData()
		{
			Scribe_References.Look<Thing>(ref this.skullspike, "skullspike", false);
			Scribe_Values.Look<int>(ref this.tickSighted, "tickSighted", 0, false);
		}

		// Token: 0x0400DA33 RID: 55859
		public Thing skullspike;

		// Token: 0x0400DA34 RID: 55860
		public int tickSighted;

		// Token: 0x0400DA35 RID: 55861
		public const int MaxSightingAge = 1800;

		// Token: 0x0400DA36 RID: 55862
		public const int CheckRadius = 10;

		// Token: 0x0400DA37 RID: 55863
		public const int CheckIntervalTicks = 60;
	}
}
