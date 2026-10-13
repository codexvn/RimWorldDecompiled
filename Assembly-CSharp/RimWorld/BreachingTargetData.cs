using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013F4 RID: 5108
	public class BreachingTargetData : IExposable
	{
		// Token: 0x06007C8F RID: 31887 RVA: 0x000024DF File Offset: 0x000006DF
		public BreachingTargetData()
		{
		}

		// Token: 0x06007C90 RID: 31888 RVA: 0x0025846D File Offset: 0x0025666D
		public BreachingTargetData(Thing target, IntVec3 firingPosition)
		{
			this.target = target;
			this.firingPosition = firingPosition;
		}

		// Token: 0x06007C91 RID: 31889 RVA: 0x00258483 File Offset: 0x00256683
		public void ExposeData()
		{
			Scribe_References.Look<Thing>(ref this.target, "target", false);
			Scribe_Values.Look<IntVec3>(ref this.firingPosition, "firingPosition", IntVec3.Invalid, false);
		}

		// Token: 0x040053F9 RID: 21497
		public Thing target;

		// Token: 0x040053FA RID: 21498
		public IntVec3 firingPosition;
	}
}
