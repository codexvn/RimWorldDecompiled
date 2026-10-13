using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D52 RID: 11602
	public class FocusStrengthOffset_ThroneSatisfiesRequirements : FocusStrengthOffset
	{
		// Token: 0x17002977 RID: 10615
		// (get) Token: 0x060104D5 RID: 66773 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool DependsOnPawn
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060104D6 RID: 66774 RVA: 0x004B555D File Offset: 0x004B375D
		public override string GetExplanation(Thing parent)
		{
			return this.GetExplanationAbstract(null);
		}

		// Token: 0x060104D7 RID: 66775 RVA: 0x004B5566 File Offset: 0x004B3766
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			return "StatsReport_SatisfiesTitle".Translate() + ": " + this.offset.ToStringWithSign("0%");
		}

		// Token: 0x060104D8 RID: 66776 RVA: 0x004B537B File Offset: 0x004B357B
		public override float GetOffset(Thing parent, Pawn user = null)
		{
			return this.offset;
		}

		// Token: 0x060104D9 RID: 66777 RVA: 0x004B5598 File Offset: 0x004B3798
		public override bool CanApply(Thing parent, Pawn user = null)
		{
			if (user == null)
			{
				return false;
			}
			Pawn_RoyaltyTracker royalty = user.royalty;
			bool? flag = (royalty != null) ? new bool?(royalty.AnyUnmetThroneroomRequirements()) : null;
			bool flag2 = false;
			return flag.GetValueOrDefault() == flag2 & flag != null;
		}
	}
}
