using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020027F2 RID: 10226
	public abstract class CompRitualEffect_IntervalSpawn : RitualVisualEffectComp
	{
		// Token: 0x1700222A RID: 8746
		// (get) Token: 0x0600DFD9 RID: 57305 RVA: 0x0041BC74 File Offset: 0x00419E74
		protected CompProperties_RitualEffectIntervalSpawn Props
		{
			get
			{
				return (CompProperties_RitualEffectIntervalSpawn)this.props;
			}
		}

		// Token: 0x0600DFDA RID: 57306 RVA: 0x0041BC84 File Offset: 0x00419E84
		public override bool ShouldSpawnNow(LordJob_Ritual ritual)
		{
			return (this.Props.delay <= 0 || this.ticksPassed >= this.Props.delay) && (this.Props.maxBursts <= 0 || this.burstsDone < this.Props.maxBursts) && (this.lastSpawnTick == -1 || GenTicks.TicksGame - this.lastSpawnTick >= this.Props.spawnIntervalTicks);
		}

		// Token: 0x0600DFDB RID: 57307 RVA: 0x0041BCFE File Offset: 0x00419EFE
		public override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			this.ticksPassed += delta;
		}

		// Token: 0x0600DFDC RID: 57308 RVA: 0x0041BD15 File Offset: 0x00419F15
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.lastSpawnTick, "lastSpawnTick", -1, false);
			Scribe_Values.Look<int>(ref this.burstsDone, "burstsDone", 0, false);
			Scribe_Values.Look<int>(ref this.ticksPassed, "ticksPassed", 0, false);
		}

		// Token: 0x040096A9 RID: 38569
		public int lastSpawnTick = -1;

		// Token: 0x040096AA RID: 38570
		public int ticksPassed;

		// Token: 0x040096AB RID: 38571
		protected int burstsDone;
	}
}
