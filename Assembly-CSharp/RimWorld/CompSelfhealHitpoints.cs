using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002DF9 RID: 11769
	public class CompSelfhealHitpoints : ThingComp
	{
		// Token: 0x17002A56 RID: 10838
		// (get) Token: 0x060108FE RID: 67838 RVA: 0x004C6550 File Offset: 0x004C4750
		public CompProperties_SelfhealHitpoints Props
		{
			get
			{
				return (CompProperties_SelfhealHitpoints)this.props;
			}
		}

		// Token: 0x060108FF RID: 67839 RVA: 0x004C655D File Offset: 0x004C475D
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksPassedSinceLastHeal, "ticksPassedSinceLastHeal", 0, false);
		}

		// Token: 0x06010900 RID: 67840 RVA: 0x004C6571 File Offset: 0x004C4771
		public override void CompTickInterval(int delta)
		{
			this.TickInterval(delta);
		}

		// Token: 0x06010901 RID: 67841 RVA: 0x004C657A File Offset: 0x004C477A
		public override void CompTickRare()
		{
			this.TickInterval(250);
		}

		// Token: 0x06010902 RID: 67842 RVA: 0x004C6587 File Offset: 0x004C4787
		public override void CompTickLong()
		{
			this.TickInterval(2000);
		}

		// Token: 0x06010903 RID: 67843 RVA: 0x004C6594 File Offset: 0x004C4794
		private void TickInterval(int ticks)
		{
			this.ticksPassedSinceLastHeal += ticks;
			if (this.ticksPassedSinceLastHeal < this.Props.ticksPerHeal)
			{
				return;
			}
			this.ticksPassedSinceLastHeal = 0;
			if (this.parent.HitPoints < this.parent.MaxHitPoints)
			{
				ThingWithComps parent = this.parent;
				int hitPoints = parent.HitPoints;
				parent.HitPoints = hitPoints + 1;
			}
		}

		// Token: 0x0400ACA4 RID: 44196
		public int ticksPassedSinceLastHeal;
	}
}
