using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020027B2 RID: 10162
	public class RitualRoleOrganizer : RitualRole
	{
		// Token: 0x0600DEB8 RID: 57016 RVA: 0x00417ECC File Offset: 0x004160CC
		public override bool AppliesToPawn(Pawn p, out string reason, TargetInfo selectedTarget, LordJob_Ritual ritual = null, RitualRoleAssignments assignments = null, Precept_Ritual precept = null, bool skipReason = false)
		{
			if (!base.AppliesIfChild(p, out reason, skipReason))
			{
				return false;
			}
			if (ritual == null)
			{
				if (assignments == null || !assignments.Required(p))
				{
					if (!skipReason)
					{
						reason = "MessageRitualRoleCannotReplaceRequiredPawn".Translate();
					}
					return false;
				}
				return true;
			}
			else
			{
				if (p != ritual.Organizer)
				{
					if (!skipReason)
					{
						reason = "MessageRitualRoleCannotReplaceRequiredPawn".Translate();
					}
					return false;
				}
				return true;
			}
		}

		// Token: 0x0600DEB9 RID: 57017 RVA: 0x00417DBD File Offset: 0x00415FBD
		public override bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn p = null, bool skipReason = false)
		{
			reason = null;
			return false;
		}
	}
}
