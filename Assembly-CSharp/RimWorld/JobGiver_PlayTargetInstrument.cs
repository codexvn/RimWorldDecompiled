using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200144B RID: 5195
	public class JobGiver_PlayTargetInstrument : ThinkNode_JobGiver
	{
		// Token: 0x06007DCC RID: 32204 RVA: 0x0025E364 File Offset: 0x0025C564
		protected override Job TryGiveJob(Pawn pawn)
		{
			PawnDuty duty = pawn.mindState.duty;
			Building_MusicalInstrument building_MusicalInstrument = ((duty != null) ? duty.focusSecond.Thing : null) as Building_MusicalInstrument;
			if (building_MusicalInstrument == null || !building_MusicalInstrument.Spawned)
			{
				return null;
			}
			if (!GatheringWorker_Concert.InstrumentAccessible(building_MusicalInstrument, pawn))
			{
				return null;
			}
			LordJob_Ritual lordJob_Ritual = pawn.GetLord().LordJob as LordJob_Ritual;
			Job job = JobMaker.MakeJob(JobDefOf.Play_MusicalInstrument, building_MusicalInstrument, building_MusicalInstrument.InteractionCell);
			job.doUntilGatheringEnded = true;
			if (lordJob_Ritual != null)
			{
				job.expiryInterval = lordJob_Ritual.DurationTicks;
			}
			else
			{
				job.expiryInterval = 2000;
			}
			return job;
		}
	}
}
