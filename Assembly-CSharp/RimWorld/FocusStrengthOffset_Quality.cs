using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D50 RID: 11600
	public class FocusStrengthOffset_Quality : FocusStrengthOffset_Curve
	{
		// Token: 0x17002974 RID: 10612
		// (get) Token: 0x060104CC RID: 66764 RVA: 0x00002501 File Offset: 0x00000701
		public override bool NeedsToBeSpawned
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060104CD RID: 66765 RVA: 0x004B54D0 File Offset: 0x004B36D0
		protected override float SourceValue(Thing parent)
		{
			QualityCategory qualityCategory;
			parent.TryGetQuality(out qualityCategory);
			return (float)qualityCategory;
		}

		// Token: 0x060104CE RID: 66766 RVA: 0x004B54E8 File Offset: 0x004B36E8
		public override float MaxOffset(Thing parent = null)
		{
			if (parent != null)
			{
				return 0f;
			}
			return base.MaxOffset(null);
		}

		// Token: 0x060104CF RID: 66767 RVA: 0x004B54FC File Offset: 0x004B36FC
		public override float MinOffset(Thing parent = null)
		{
			if (parent != null)
			{
				return this.GetOffset(parent, null);
			}
			return this.curve[0].y;
		}

		// Token: 0x17002975 RID: 10613
		// (get) Token: 0x060104D0 RID: 66768 RVA: 0x004B5529 File Offset: 0x004B3729
		protected override string ExplanationKey
		{
			get
			{
				return "StatsReport_FromQuality";
			}
		}
	}
}
