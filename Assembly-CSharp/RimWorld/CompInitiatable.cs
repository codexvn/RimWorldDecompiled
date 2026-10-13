using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F42 RID: 12098
	public class CompInitiatable : ThingComp
	{
		// Token: 0x17002C19 RID: 11289
		// (get) Token: 0x060110F8 RID: 69880 RVA: 0x004E6E72 File Offset: 0x004E5072
		public bool Initiated
		{
			get
			{
				return this.Delay <= 0 || (this.parent.spawnedTick >= 0 && Find.TickManager.TicksGame >= this.parent.spawnedTick + this.Delay);
			}
		}

		// Token: 0x17002C1A RID: 11290
		// (get) Token: 0x060110F9 RID: 69881 RVA: 0x004E6EB0 File Offset: 0x004E50B0
		private int Delay
		{
			get
			{
				if (this.initiationDelayTicksOverride <= 0)
				{
					return this.Props.initiationDelayTicks;
				}
				return this.initiationDelayTicksOverride;
			}
		}

		// Token: 0x17002C1B RID: 11291
		// (get) Token: 0x060110FA RID: 69882 RVA: 0x004E6ECD File Offset: 0x004E50CD
		private CompProperties_Initiatable Props
		{
			get
			{
				return (CompProperties_Initiatable)this.props;
			}
		}

		// Token: 0x060110FB RID: 69883 RVA: 0x004E6EDC File Offset: 0x004E50DC
		public override string CompInspectStringExtra()
		{
			if (!this.Initiated)
			{
				return "InitiatesIn".Translate() + ": " + (this.parent.spawnedTick + this.Delay - Find.TickManager.TicksGame).ToStringTicksToPeriod(true, false, true, true, false);
			}
			return base.CompInspectStringExtra();
		}

		// Token: 0x060110FC RID: 69884 RVA: 0x004E6F3D File Offset: 0x004E513D
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.initiationDelayTicksOverride, "initiationDelayTicksOverride", 0, false);
		}

		// Token: 0x0400B132 RID: 45362
		public int initiationDelayTicksOverride;
	}
}
