using System;

namespace Verse
{
	// Token: 0x02000692 RID: 1682
	public class HediffComp_ChanceToRemove : HediffComp
	{
		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x06002A67 RID: 10855 RVA: 0x000DF5AC File Offset: 0x000DD7AC
		public HediffCompProperties_ChanceToRemove Props
		{
			get
			{
				return (HediffCompProperties_ChanceToRemove)this.props;
			}
		}

		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x06002A68 RID: 10856 RVA: 0x000DF5B9 File Offset: 0x000DD7B9
		public override bool CompShouldRemove
		{
			get
			{
				return base.CompShouldRemove || (this.removeNextInterval && this.currentInterval <= 0);
			}
		}

		// Token: 0x06002A69 RID: 10857 RVA: 0x000DF5DC File Offset: 0x000DD7DC
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			if (this.CompShouldRemove)
			{
				return;
			}
			if (this.currentInterval > 0)
			{
				this.currentInterval -= delta;
				return;
			}
			if (Rand.Chance(this.Props.chance))
			{
				this.removeNextInterval = true;
				this.currentInterval = Rand.Range(0, this.Props.intervalTicks);
				return;
			}
			this.currentInterval = this.Props.intervalTicks;
		}

		// Token: 0x06002A6A RID: 10858 RVA: 0x000DF64C File Offset: 0x000DD84C
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.currentInterval, "currentInterval", 0, false);
			Scribe_Values.Look<bool>(ref this.removeNextInterval, "removeNextInterval", false, false);
		}

		// Token: 0x06002A6B RID: 10859 RVA: 0x000DF672 File Offset: 0x000DD872
		public override string CompDebugString()
		{
			return string.Format("currentInterval: {0}\nremove: {1}", this.currentInterval, this.removeNextInterval);
		}

		// Token: 0x04002261 RID: 8801
		public int currentInterval;

		// Token: 0x04002262 RID: 8802
		public bool removeNextInterval;
	}
}
