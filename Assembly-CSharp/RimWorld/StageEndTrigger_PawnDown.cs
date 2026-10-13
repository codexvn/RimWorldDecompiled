using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020026AB RID: 9899
	public class StageEndTrigger_PawnDown : StageEndTrigger
	{
		// Token: 0x0600D92B RID: 55595 RVA: 0x004051B4 File Offset: 0x004033B4
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
			return new Trigger_TickCondition(() => pawn.Downed, 1);
		}

		// Token: 0x0600D92C RID: 55596 RVA: 0x0040522B File Offset: 0x0040342B
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.roleId, "roleId", null, false);
		}

		// Token: 0x04009329 RID: 37673
		[NoTranslate]
		public string roleId;
	}
}
