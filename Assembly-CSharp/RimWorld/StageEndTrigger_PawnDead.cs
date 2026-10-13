using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020026A9 RID: 9897
	public class StageEndTrigger_PawnDead : StageEndTrigger
	{
		// Token: 0x0600D925 RID: 55589 RVA: 0x00405104 File Offset: 0x00403304
		public override Trigger MakeTrigger(LordJob_Ritual ritual, TargetInfo spot, IEnumerable<TargetInfo> foci, RitualStage stage)
		{
			if (ritual.Ritual.behavior.def.roles.FirstOrDefault((RitualRole r) => r.id == this.roleId) == null)
			{
				return null;
			}
			Pawn pawn = ritual.assignments.FirstAssignedPawn(this.roleId);
			if (pawn == null)
			{
				return null;
			}
			return new Trigger_TickCondition(() => pawn.Dead, 1);
		}

		// Token: 0x0600D926 RID: 55590 RVA: 0x0040517B File Offset: 0x0040337B
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.roleId, "roleId", null, false);
		}

		// Token: 0x04009326 RID: 37670
		[NoTranslate]
		public string roleId;
	}
}
