using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020027BF RID: 10175
	public class RitualRole_Mother : RitualRoleForced
	{
		// Token: 0x0600DEE4 RID: 57060 RVA: 0x004189C8 File Offset: 0x00416BC8
		public override bool AppliesToPawn(Pawn p, out string reason, TargetInfo selectedTarget, LordJob_Ritual ritual = null, RitualRoleAssignments assignments = null, Precept_Ritual precept = null, bool skipReason = false)
		{
			reason = null;
			if (base.AppliesToPawn(p, out reason, selectedTarget, ritual, assignments, precept, skipReason))
			{
				return true;
			}
			if (p.health.hediffSet.HasHediff(HediffDefOf.PregnancyLabor, false))
			{
				if (selectedTarget.IsValid && PregnancyUtility.IsUsableBedFor(p, p, (Building_Bed)selectedTarget.Thing))
				{
					return true;
				}
				reason = "CantUseThisBed".Translate();
			}
			else
			{
				reason = "NotInLabor".Translate();
			}
			return false;
		}
	}
}
