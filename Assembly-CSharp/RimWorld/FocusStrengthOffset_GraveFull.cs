using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D53 RID: 11603
	public class FocusStrengthOffset_GraveFull : FocusStrengthOffset
	{
		// Token: 0x060104DB RID: 66779 RVA: 0x004B55E0 File Offset: 0x004B37E0
		public override string GetExplanation(Thing parent)
		{
			if (this.CanApply(parent, null))
			{
				Building_Grave building_Grave = parent as Building_Grave;
				return "StatsReport_GraveFull".Translate(building_Grave.Corpse.InnerPawn.LabelShortCap) + ": " + this.GetOffset(parent, null).ToStringWithSign("0%");
			}
			return this.GetExplanationAbstract(null);
		}

		// Token: 0x060104DC RID: 66780 RVA: 0x004B564B File Offset: 0x004B384B
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			return "StatsReport_GraveFullAbstract".Translate() + ": " + this.offset.ToStringWithSign("0%");
		}

		// Token: 0x060104DD RID: 66781 RVA: 0x004B537B File Offset: 0x004B357B
		public override float GetOffset(Thing parent, Pawn user = null)
		{
			return this.offset;
		}

		// Token: 0x060104DE RID: 66782 RVA: 0x004B567C File Offset: 0x004B387C
		public override bool CanApply(Thing parent, Pawn user = null)
		{
			if (parent.Spawned)
			{
				Building_Grave building_Grave = parent as Building_Grave;
				if (building_Grave != null && building_Grave.HasCorpse)
				{
					return building_Grave.Corpse.InnerPawn.RaceProps.Humanlike;
				}
			}
			return false;
		}
	}
}
