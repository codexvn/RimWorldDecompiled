using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020027B1 RID: 10161
	public class RitualRolePrisonerOrSlave : RitualRole
	{
		// Token: 0x0600DEB4 RID: 57012 RVA: 0x00417DC4 File Offset: 0x00415FC4
		public override bool AppliesToPawn(Pawn p, out string reason, TargetInfo selectedTarget, LordJob_Ritual ritual = null, RitualRoleAssignments assignments = null, Precept_Ritual precept = null, bool skipReason = false)
		{
			if (!base.AppliesIfChild(p, out reason, skipReason))
			{
				return false;
			}
			if (this.disallowWildManPrisoner && p.IsWildMan() && p.IsPrisoner)
			{
				if (!skipReason)
				{
					reason = "MessageRitualRoleMustNotBeImprisonedWildMan".Translate(base.LabelCap);
				}
				return false;
			}
			if (!p.IsPrisonerOfColony && !p.IsSlaveOfColony)
			{
				if (!skipReason)
				{
					reason = "MessageRitualRoleMustBePrisonerOrSlave".Translate(base.LabelCap);
				}
				return false;
			}
			if (this.mustBeCapableToFight && (p.WorkTagIsDisabled(WorkTags.Violent) || !p.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation)))
			{
				if (!skipReason)
				{
					reason = "MessageRitualRoleMustBeCapableOfFighting".Translate(p);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600DEB5 RID: 57013 RVA: 0x00417DBD File Offset: 0x00415FBD
		public override bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn p = null, bool skipReason = false)
		{
			reason = null;
			return false;
		}

		// Token: 0x0600DEB6 RID: 57014 RVA: 0x00417E91 File Offset: 0x00416091
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.mustBeCapableToFight, "mustBeCapableToFight", false, false);
			Scribe_Values.Look<bool>(ref this.disallowWildManPrisoner, "disallowWildManPrisoner", false, false);
		}

		// Token: 0x04009613 RID: 38419
		public bool mustBeCapableToFight;

		// Token: 0x04009614 RID: 38420
		public bool disallowWildManPrisoner = true;
	}
}
