using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020027B9 RID: 10169
	public class RitualRoleColonist : RitualRole
	{
		// Token: 0x0600DED2 RID: 57042 RVA: 0x00418400 File Offset: 0x00416600
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
			if (this.requiredWorkType != null && p.WorkTypeIsDisabled(this.requiredWorkType))
			{
				if (!skipReason)
				{
					reason = "MessageRitualRoleMustBeCapableOfGeneric".Translate(base.LabelCap, this.requiredWorkType.gerundLabel);
				}
				return false;
			}
			if (this.usedSkill != null && p.skills.GetSkill(this.usedSkill).TotallyDisabled)
			{
				if (!skipReason)
				{
					reason = "MessageRitualRoleMustBeCapableOfGeneric".Translate(base.LabelCap, this.usedSkill.label);
				}
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
			GuestStatus? guestStatus = p.GuestStatus;
			GuestStatus guestStatus2 = GuestStatus.Prisoner;
			if (guestStatus.GetValueOrDefault() == guestStatus2 & guestStatus != null)
			{
				if (!skipReason)
				{
					reason = "MessageRitualRoleMustBeFree".Translate(base.Label);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600DED3 RID: 57043 RVA: 0x00417DBD File Offset: 0x00415FBD
		public override bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn p = null, bool skipReason = false)
		{
			reason = null;
			return false;
		}

		// Token: 0x0600DED4 RID: 57044 RVA: 0x00418554 File Offset: 0x00416754
		protected override int PawnDesirability(Pawn pawn)
		{
			int num = base.PawnDesirability(pawn);
			if (this.usedStat != null)
			{
				if (!this.usedStat.Worker.IsDisabledFor(pawn))
				{
					num += Mathf.RoundToInt(100f * pawn.GetStatValue(this.usedStat, true, -1));
				}
			}
			else if (this.usedSkill != null)
			{
				num += 2 * pawn.skills.GetSkill(this.usedSkill).Level;
			}
			return num;
		}

		// Token: 0x0600DED5 RID: 57045 RVA: 0x004185C6 File Offset: 0x004167C6
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<WorkTypeDef>(ref this.requiredWorkType, "requiredWorkType");
		}

		// Token: 0x04009619 RID: 38425
		public WorkTypeDef requiredWorkType;

		// Token: 0x0400961A RID: 38426
		public SkillDef usedSkill;

		// Token: 0x0400961B RID: 38427
		public StatDef usedStat;
	}
}
