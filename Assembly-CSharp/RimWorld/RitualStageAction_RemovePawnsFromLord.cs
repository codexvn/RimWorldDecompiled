using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020027CA RID: 10186
	public class RitualStageAction_RemovePawnsFromLord : RitualStageAction
	{
		// Token: 0x0600DF06 RID: 57094 RVA: 0x00418EE4 File Offset: 0x004170E4
		public override void Apply(LordJob_Ritual ritual)
		{
			foreach (string roleId in this.roleIds)
			{
				foreach (Pawn p in ritual.assignments.AssignedPawns(roleId))
				{
					ritual.lord.RemovePawn(p);
				}
			}
		}

		// Token: 0x0600DF07 RID: 57095 RVA: 0x00418F78 File Offset: 0x00417178
		public override void ExposeData()
		{
			Scribe_Collections.Look<string>(ref this.roleIds, "roleIds", LookMode.Undefined, Array.Empty<object>());
		}

		// Token: 0x0400962B RID: 38443
		public List<string> roleIds = new List<string>();
	}
}
