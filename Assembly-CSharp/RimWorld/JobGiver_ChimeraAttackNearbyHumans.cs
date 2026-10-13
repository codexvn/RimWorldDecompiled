using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200147D RID: 5245
	public class JobGiver_ChimeraAttackNearbyHumans : ThinkNode_JobGiver
	{
		// Token: 0x06007E5B RID: 32347 RVA: 0x002607F4 File Offset: 0x0025E9F4
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn_MindState mindState = pawn.mindState;
			DutyDef dutyDef;
			if (mindState == null)
			{
				dutyDef = null;
			}
			else
			{
				PawnDuty duty = mindState.duty;
				dutyDef = ((duty != null) ? duty.def : null);
			}
			if (dutyDef == DutyDefOf.ChimeraStalkFlee)
			{
				return null;
			}
			List<Pawn> allHumanlikeSpawned = pawn.Map.mapPawns.AllHumanlikeSpawned;
			for (int i = 0; i < allHumanlikeSpawned.Count; i++)
			{
				if (!allHumanlikeSpawned[i].DeadOrDowned && pawn.Position.InHorDistOf(allHumanlikeSpawned[i].Position, this.attackRadius))
				{
					return JobMaker.MakeJob(JobDefOf.ChimeraSwitchToAttackMode);
				}
			}
			return null;
		}

		// Token: 0x040054BF RID: 21695
		public float attackRadius = 10f;
	}
}
