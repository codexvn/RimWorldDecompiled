using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020027BD RID: 10173
	public class RitualRoleIdeoRoleChanger : RitualRole
	{
		// Token: 0x0600DEDF RID: 57055 RVA: 0x004188C0 File Offset: 0x00416AC0
		public override bool AppliesToPawn(Pawn p, out string reason, TargetInfo selectedTarget, LordJob_Ritual ritual = null, RitualRoleAssignments assignments = null, Precept_Ritual precept = null, bool skipReason = false)
		{
			if (!base.AppliesIfChild(p, out reason, skipReason))
			{
				return false;
			}
			if (p.Ideo == null)
			{
				return false;
			}
			if (!p.IsFreeNonSlaveColonist)
			{
				if (!skipReason)
				{
					reason = "MessageRitualRoleMustBeFreeColonist".Translate(base.Label);
				}
				return false;
			}
			if (p.Ideo.GetRole(p) == null && !RitualUtility.AllRolesForPawn(p).Any((Precept_Role r) => r.RequirementsMet(p)))
			{
				reason = "MessageRitualNoRolesAvailable".Translate(p);
				return false;
			}
			if (!Faction.OfPlayer.ideos.Has(p.Ideo))
			{
				reason = "MessageRitualNotOfPlayerIdeo".Translate(p);
				return false;
			}
			return true;
		}

		// Token: 0x0600DEE0 RID: 57056 RVA: 0x00417DBD File Offset: 0x00415FBD
		public override bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn p = null, bool skipReason = false)
		{
			reason = null;
			return false;
		}
	}
}
