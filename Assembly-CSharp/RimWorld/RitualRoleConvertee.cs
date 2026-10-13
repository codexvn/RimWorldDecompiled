using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020027BB RID: 10171
	public class RitualRoleConvertee : RitualRole
	{
		// Token: 0x0600DED9 RID: 57049 RVA: 0x00418718 File Offset: 0x00416918
		public override bool AppliesToPawn(Pawn p, out string reason, TargetInfo selectedTarget, LordJob_Ritual ritual = null, RitualRoleAssignments assignments = null, Precept_Ritual precept = null, bool skipReason = false)
		{
			if (!base.AppliesIfChild(p, out reason, skipReason))
			{
				return false;
			}
			if (!p.RaceProps.Humanlike)
			{
				if (!skipReason)
				{
					reason = "MessageRitualRoleMustBeHumanlike".Translate(base.Label);
				}
				return false;
			}
			if ((assignments == null || !assignments.Forced(p)) && p.Ideo == ((assignments != null) ? assignments.Ritual.ideo : null))
			{
				if (!skipReason)
				{
					reason = "MessageRitualPawnIsAlreadyBelievingIdeo".Translate(p, Find.ActiveLanguageWorker.WithIndefiniteArticle(p.Ideo.memberName, p.gender, false, false));
				}
				return false;
			}
			if (p.DevelopmentalStage.Baby())
			{
				if (!skipReason)
				{
					reason = "MessageRitualRoleCannotBeABaby".Translate(base.Label);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600DEDA RID: 57050 RVA: 0x00417DBD File Offset: 0x00415FBD
		public override bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn p = null, bool skipReason = false)
		{
			reason = null;
			return false;
		}
	}
}
