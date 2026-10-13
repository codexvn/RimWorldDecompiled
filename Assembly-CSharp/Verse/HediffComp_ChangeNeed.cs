using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000698 RID: 1688
	public class HediffComp_ChangeNeed : HediffComp
	{
		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x06002A7F RID: 10879 RVA: 0x000DFAF3 File Offset: 0x000DDCF3
		public HediffCompProperties_ChangeNeed Props
		{
			get
			{
				return (HediffCompProperties_ChangeNeed)this.props;
			}
		}

		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x06002A80 RID: 10880 RVA: 0x000DFB00 File Offset: 0x000DDD00
		private Need Need
		{
			get
			{
				if (this.needCached == null)
				{
					base.Pawn.needs.TryGetNeed<Need>(out this.needCached);
				}
				return this.needCached;
			}
		}

		// Token: 0x06002A81 RID: 10881 RVA: 0x000DFB27 File Offset: 0x000DDD27
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			if (this.Need != null)
			{
				this.Need.CurLevelPercentage += this.Props.percentPerDay / 60000f * (float)delta;
			}
		}

		// Token: 0x04002272 RID: 8818
		private Need needCached;
	}
}
