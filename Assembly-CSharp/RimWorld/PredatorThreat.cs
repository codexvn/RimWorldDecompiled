using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020025A5 RID: 9637
	public class PredatorThreat : IExposable
	{
		// Token: 0x17002080 RID: 8320
		// (get) Token: 0x0600D32B RID: 54059 RVA: 0x003E30C2 File Offset: 0x003E12C2
		public bool Expired
		{
			get
			{
				return !this.predator.Spawned || Find.TickManager.TicksGame >= this.lastAttackTicks + 600;
			}
		}

		// Token: 0x0600D32C RID: 54060 RVA: 0x003E30EE File Offset: 0x003E12EE
		public void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.predator, "predator", false);
			Scribe_Values.Look<int>(ref this.lastAttackTicks, "lastAttackTicks", 0, false);
		}

		// Token: 0x04008F48 RID: 36680
		public Pawn predator;

		// Token: 0x04008F49 RID: 36681
		public int lastAttackTicks;

		// Token: 0x04008F4A RID: 36682
		private const int ExpireAfterTicks = 600;
	}
}
