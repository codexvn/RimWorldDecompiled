using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020027B0 RID: 10160
	public class RitualRolePrisoner : RitualRole
	{
		// Token: 0x0600DEB1 RID: 57009 RVA: 0x00417D82 File Offset: 0x00415F82
		public override bool AppliesToPawn(Pawn p, out string reason, TargetInfo selectedTarget, LordJob_Ritual ritual = null, RitualRoleAssignments assignments = null, Precept_Ritual precept = null, bool skipReason = false)
		{
			if (!base.AppliesIfChild(p, out reason, skipReason))
			{
				return false;
			}
			if (p.IsPrisonerOfColony)
			{
				return true;
			}
			if (!skipReason)
			{
				reason = "MessageRitualRoleMustBePrisoner".Translate(base.LabelCap);
			}
			return false;
		}

		// Token: 0x0600DEB2 RID: 57010 RVA: 0x00417DBD File Offset: 0x00415FBD
		public override bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn p = null, bool skipReason = false)
		{
			reason = null;
			return false;
		}
	}
}
