using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020027CC RID: 10188
	public abstract class ActionOnTick : IExposable
	{
		// Token: 0x0600DF0C RID: 57100
		public abstract void Apply(LordJob_Ritual ritual);

		// Token: 0x0600DF0D RID: 57101 RVA: 0x00419089 File Offset: 0x00417289
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.tick, "tick", 0, false);
		}

		// Token: 0x0400962E RID: 38446
		public int tick;
	}
}
