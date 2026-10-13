using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001961 RID: 6497
	public class CompDestroyAfterEffect : ThingComp
	{
		// Token: 0x17001666 RID: 5734
		// (get) Token: 0x0600939F RID: 37791 RVA: 0x002B6DB2 File Offset: 0x002B4FB2
		public CompProperties_DestroyAfterEffect Props
		{
			get
			{
				return (CompProperties_DestroyAfterEffect)this.props;
			}
		}

		// Token: 0x060093A0 RID: 37792 RVA: 0x002B6DC0 File Offset: 0x002B4FC0
		public override void CompTick()
		{
			int ticksGame = Find.TickManager.TicksGame;
			if (this.effecter == null)
			{
				this.effecter = this.Props.effecterDef.SpawnMaintained(this.parent.PositionHeld, this.parent.Map, 1f);
				if (this.destructionTick < 0)
				{
					this.destructionTick = ticksGame + this.Props.effecterDef.maintainTicks;
				}
			}
			if (ticksGame >= this.destructionTick)
			{
				this.parent.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x060093A1 RID: 37793 RVA: 0x002B6E47 File Offset: 0x002B5047
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.destructionTick, "destructionTick", 0, false);
		}

		// Token: 0x04005EFD RID: 24317
		public int destructionTick = int.MinValue;

		// Token: 0x04005EFE RID: 24318
		private Effecter effecter;
	}
}
