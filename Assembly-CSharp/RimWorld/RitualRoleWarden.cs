using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020027B6 RID: 10166
	public class RitualRoleWarden : RitualRole
	{
		// Token: 0x0600DEC7 RID: 57031 RVA: 0x00418188 File Offset: 0x00416388
		public override bool AppliesToPawn(Pawn p, out string reason, TargetInfo selectedTarget, LordJob_Ritual ritual = null, RitualRoleAssignments assignments = null, Precept_Ritual precept = null, bool skipReason = false)
		{
			if (!base.AppliesIfChild(p, out reason, skipReason))
			{
				return false;
			}
			if (!p.Faction.IsPlayerSafe())
			{
				if (!skipReason)
				{
					reason = "MessageRitualRoleMustBeColonist".Translate(base.Label);
				}
				return false;
			}
			if (!p.RaceProps.Humanlike)
			{
				if (!skipReason)
				{
					reason = "MessageRitualRoleMustBeHumanlike".Translate(base.LabelCap);
				}
				return false;
			}
			if (!p.workSettings.WorkIsActive(WorkTypeDefOf.Warden))
			{
				if (!skipReason)
				{
					reason = "MessageRitualRoleMustBeCapableOfWardening".Translate(p);
				}
				return false;
			}
			if (p.WorkTagIsDisabled(WorkTags.Violent))
			{
				if (!skipReason)
				{
					reason = "MessageRitualRoleMustBeCapableOfViolence".Translate(p);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600DEC8 RID: 57032 RVA: 0x00417DBD File Offset: 0x00415FBD
		public override bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn p = null, bool skipReason = false)
		{
			reason = null;
			return false;
		}
	}
}
