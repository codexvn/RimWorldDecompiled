using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D5E RID: 11614
	public class FocusStrengthOffset_OccupiedHoldingPlatform : FocusStrengthOffset
	{
		// Token: 0x06010514 RID: 66836 RVA: 0x004B6258 File Offset: 0x004B4458
		public override string GetExplanation(Thing parent)
		{
			if (this.CanApply(parent, null))
			{
				return "StatsReport_Occupied".Translate() + ": " + this.GetOffset(parent, null).ToStringWithSign("0%");
			}
			return this.GetExplanationAbstract(null);
		}

		// Token: 0x06010515 RID: 66837 RVA: 0x004B62A7 File Offset: 0x004B44A7
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			return "StatsReport_Occupied".Translate() + ": " + this.offset.ToStringWithSign("0%");
		}

		// Token: 0x06010516 RID: 66838 RVA: 0x004B537B File Offset: 0x004B357B
		public override float GetOffset(Thing parent, Pawn user = null)
		{
			return this.offset;
		}

		// Token: 0x06010517 RID: 66839 RVA: 0x004B62D8 File Offset: 0x004B44D8
		public override bool CanApply(Thing parent, Pawn user = null)
		{
			if (!ModLister.CheckAnomaly("FocusStrengthOffset_OccupiedHoldingPlatform"))
			{
				return false;
			}
			Building_HoldingPlatform building_HoldingPlatform = parent as Building_HoldingPlatform;
			return building_HoldingPlatform != null && building_HoldingPlatform.Occupied;
		}
	}
}
