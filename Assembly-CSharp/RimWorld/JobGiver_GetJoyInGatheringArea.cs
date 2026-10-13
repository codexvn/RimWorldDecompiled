using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001441 RID: 5185
	public class JobGiver_GetJoyInGatheringArea : JobGiver_GetJoy
	{
		// Token: 0x06007DB0 RID: 32176 RVA: 0x0025DBDC File Offset: 0x0025BDDC
		protected override Job TryGiveJobFromJoyGiverDefDirect(JoyGiverDef def, Pawn pawn)
		{
			if (pawn.mindState.duty == null)
			{
				return null;
			}
			if (pawn.needs.joy == null)
			{
				return null;
			}
			if (pawn.needs.joy.CurLevelPercentage > 0.92f)
			{
				return null;
			}
			IntVec3 cell = pawn.mindState.duty.focus.Cell;
			return def.Worker.TryGiveJobInGatheringArea(pawn, cell, this.desiredRadius);
		}

		// Token: 0x06007DB1 RID: 32177 RVA: 0x0025DC49 File Offset: 0x0025BE49
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.needs.joy == null)
			{
				return null;
			}
			return base.TryGiveJob(pawn);
		}

		// Token: 0x06007DB2 RID: 32178 RVA: 0x0025DC61 File Offset: 0x0025BE61
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_GetJoyInGatheringArea jobGiver_GetJoyInGatheringArea = (JobGiver_GetJoyInGatheringArea)base.DeepCopy(resolve);
			jobGiver_GetJoyInGatheringArea.desiredRadius = this.desiredRadius;
			return jobGiver_GetJoyInGatheringArea;
		}

		// Token: 0x0400546C RID: 21612
		public float desiredRadius = -1f;
	}
}
