using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020027B3 RID: 10163
	public class RitualRoleForced : RitualRole
	{
		// Token: 0x0600DEBB RID: 57019 RVA: 0x00417F34 File Offset: 0x00416134
		public override bool AppliesToPawn(Pawn p, out string reason, TargetInfo selectedTarget, LordJob_Ritual ritual = null, RitualRoleAssignments assignments = null, Precept_Ritual precept = null, bool skipReason = false)
		{
			reason = null;
			return ritual != null && base.AppliesIfChild(p, out reason, skipReason) && ritual.assignments.ForcedRole(p).id == this.id;
		}

		// Token: 0x0600DEBC RID: 57020 RVA: 0x00417DBD File Offset: 0x00415FBD
		public override bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn p = null, bool skipReason = false)
		{
			reason = null;
			return false;
		}
	}
}
