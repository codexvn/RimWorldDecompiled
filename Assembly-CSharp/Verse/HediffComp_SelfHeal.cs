using System;

namespace Verse
{
	// Token: 0x02000700 RID: 1792
	public class HediffComp_SelfHeal : HediffComp
	{
		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x06002BE9 RID: 11241 RVA: 0x000E4A87 File Offset: 0x000E2C87
		public HediffCompProperties_SelfHeal Props
		{
			get
			{
				return (HediffCompProperties_SelfHeal)this.props;
			}
		}

		// Token: 0x06002BEA RID: 11242 RVA: 0x000E4A94 File Offset: 0x000E2C94
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksSinceHeal, "ticksSinceHeal", 0, false);
		}

		// Token: 0x06002BEB RID: 11243 RVA: 0x000E4AA8 File Offset: 0x000E2CA8
		public override void CompPostTick(ref float severityAdjustment)
		{
			this.ticksSinceHeal++;
			if (this.ticksSinceHeal > this.Props.healIntervalTicksStanding)
			{
				severityAdjustment -= this.Props.healAmount;
				this.ticksSinceHeal = 0;
			}
		}

		// Token: 0x04002355 RID: 9045
		public int ticksSinceHeal;
	}
}
