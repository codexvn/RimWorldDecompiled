using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020027BC RID: 10172
	public class RitualRoleAnimaLinker : RitualRole
	{
		// Token: 0x0600DEDC RID: 57052 RVA: 0x004187FC File Offset: 0x004169FC
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
			if (ritual != null)
			{
				if (p == ritual.Organizer)
				{
					return true;
				}
			}
			else if (assignments != null && assignments.Required(p))
			{
				return true;
			}
			if (!MeditationFocusDefOf.Natural.CanPawnUse(p) || p.GetPsylinkLevel() >= p.GetMaxPsylinkLevel())
			{
				if (!skipReason)
				{
					reason = "RitualTargetAnimaTreeMustBeCapableOfNature".Translate();
				}
				return false;
			}
			if (!p.psychicEntropy.IsPsychicallySensitive)
			{
				if (!skipReason)
				{
					reason = "RitualTargetAnimaTreeMustBePsychicallySensitive".Translate();
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600DEDD RID: 57053 RVA: 0x00417DBD File Offset: 0x00415FBD
		public override bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn p = null, bool skipReason = false)
		{
			reason = null;
			return false;
		}
	}
}
