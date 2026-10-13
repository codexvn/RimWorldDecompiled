using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020027CB RID: 10187
	public class RitualStageAction_StunPawns : RitualStageAction
	{
		// Token: 0x0600DF09 RID: 57097 RVA: 0x00418FA4 File Offset: 0x004171A4
		public override void Apply(LordJob_Ritual ritual)
		{
			foreach (string roleId in this.roleIds)
			{
				foreach (Pawn pawn in ritual.assignments.AssignedPawns(roleId))
				{
					pawn.stances.stunner.StunFor(this.stunDurationTicks, null, false, false, false);
				}
			}
		}

		// Token: 0x0600DF0A RID: 57098 RVA: 0x00419044 File Offset: 0x00417244
		public override void ExposeData()
		{
			Scribe_Collections.Look<string>(ref this.roleIds, "roleIds", LookMode.Undefined, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.stunDurationTicks, "stunDurationTicks", 0, false);
		}

		// Token: 0x0400962C RID: 38444
		public List<string> roleIds = new List<string>();

		// Token: 0x0400962D RID: 38445
		public int stunDurationTicks = 10;
	}
}
