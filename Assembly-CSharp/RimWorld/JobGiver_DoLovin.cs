using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200147F RID: 5247
	public class JobGiver_DoLovin : ThinkNode_JobGiver
	{
		// Token: 0x06007E5F RID: 32351 RVA: 0x002608B8 File Offset: 0x0025EAB8
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (Find.TickManager.TicksGame < pawn.mindState.canLovinTick)
			{
				return null;
			}
			if (pawn.CurrentBed() == null || pawn.CurrentBed().Medical || !pawn.health.capacities.CanBeAwake)
			{
				return null;
			}
			Pawn partnerInMyBed = LovePartnerRelationUtility.GetPartnerInMyBed(pawn);
			if (partnerInMyBed == null || !partnerInMyBed.health.capacities.CanBeAwake || Find.TickManager.TicksGame < partnerInMyBed.mindState.canLovinTick)
			{
				return null;
			}
			if (!pawn.CanReserve(partnerInMyBed, 1, -1, null, false) || !partnerInMyBed.CanReserve(pawn, 1, -1, null, false))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.Lovin, partnerInMyBed, pawn.CurrentBed());
		}
	}
}
