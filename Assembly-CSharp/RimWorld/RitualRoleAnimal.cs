using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020027B4 RID: 10164
	public class RitualRoleAnimal : RitualRole
	{
		// Token: 0x17002212 RID: 8722
		// (get) Token: 0x0600DEBE RID: 57022 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool Animal
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600DEBF RID: 57023 RVA: 0x00417F6C File Offset: 0x0041616C
		public override bool AppliesToPawn(Pawn p, out string reason, TargetInfo selectedTarget, LordJob_Ritual ritual = null, RitualRoleAssignments assignments = null, Precept_Ritual precept = null, bool skipReason = false)
		{
			reason = null;
			if (!p.IsAnimal)
			{
				if (!skipReason)
				{
					reason = "MessageRitualRoleMustBeAnimal".Translate(base.LabelCap);
				}
				return false;
			}
			if (p.BodySize < this.minBodySize)
			{
				if (!skipReason)
				{
					reason = "MessageRitualRoleMustHaveLargerBodySize".Translate(base.LabelCap, this.minBodySize.ToString("0.00"));
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
			return true;
		}

		// Token: 0x0600DEC0 RID: 57024 RVA: 0x00417DBD File Offset: 0x00415FBD
		public override bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn p = null, bool skipReason = false)
		{
			reason = null;
			return false;
		}

		// Token: 0x0600DEC1 RID: 57025 RVA: 0x0041801B File Offset: 0x0041621B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.minBodySize, "minBodySize", 0f, false);
		}

		// Token: 0x04009615 RID: 38421
		private float minBodySize;
	}
}
