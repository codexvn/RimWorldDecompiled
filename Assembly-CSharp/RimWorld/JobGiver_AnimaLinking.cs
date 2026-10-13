using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001479 RID: 5241
	public class JobGiver_AnimaLinking : ThinkNode_JobGiver
	{
		// Token: 0x06007E52 RID: 32338 RVA: 0x0026069C File Offset: 0x0025E89C
		protected override Job TryGiveJob(Pawn pawn)
		{
			PawnDuty duty = pawn.mindState.duty;
			if (duty == null)
			{
				return null;
			}
			if (!pawn.CanReserveAndReach(duty.focus, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, false))
			{
				return null;
			}
			Thing thing = duty.focusSecond.Thing;
			CompPsylinkable compPsylinkable = (thing != null) ? thing.TryGetComp<CompPsylinkable>() : null;
			if (compPsylinkable == null || !compPsylinkable.CanPsylink(pawn, null, true).Accepted)
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.LinkPsylinkable, duty.focusSecond, duty.focus);
		}
	}
}
