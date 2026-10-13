using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020026A7 RID: 9895
	public class StageEndTrigger_PawnOnTargetCell : StageEndTrigger
	{
		// Token: 0x0600D91F RID: 55583 RVA: 0x00404FF0 File Offset: 0x004031F0
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
			PawnStagePosition dest = ritual.PawnPositionForStage(pawn, stage);
			return new Trigger_TicksPassedAfterConditionMet(this.waitTicks, () => pawn.CanReachImmediate(dest.cell, PathEndMode.ClosestTouch) || pawn.Dead, 1);
		}

		// Token: 0x0600D920 RID: 55584 RVA: 0x00405081 File Offset: 0x00403281
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.roleId, "roleId", null, false);
			Scribe_Values.Look<int>(ref this.waitTicks, "waitTicks", 0, false);
		}

		// Token: 0x04009321 RID: 37665
		public string roleId;

		// Token: 0x04009322 RID: 37666
		public int waitTicks = 50;
	}
}
