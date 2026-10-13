using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200161B RID: 5659
	public class WorkGiver_Teach : WorkGiver_Scanner
	{
		// Token: 0x0600857F RID: 34175 RVA: 0x0027E62A File Offset: 0x0027C82A
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn teacher)
		{
			return teacher.Map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer);
		}

		// Token: 0x06008580 RID: 34176 RVA: 0x0027E641 File Offset: 0x0027C841
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !ModsConfig.BiotechActive || !SchoolUtility.CanTeachNow(pawn);
		}

		// Token: 0x06008581 RID: 34177 RVA: 0x0027E658 File Offset: 0x0027C858
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null || pawn.skills == null || pawn2.skills == null)
			{
				return false;
			}
			if (SchoolUtility.NeedsTeacher(pawn2))
			{
				Thing thing = pawn2.CurJob.GetTarget(TargetIndex.A).Thing;
				if (thing != null && pawn.CanReserveSittableOrSpot(SchoolUtility.DeskSpotTeacher(thing), false))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008582 RID: 34178 RVA: 0x0027E6B4 File Offset: 0x0027C8B4
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null)
			{
				return null;
			}
			pawn2.CurJob.SetTarget(TargetIndex.B, pawn);
			return JobMaker.MakeJob(JobDefOf.Lessongiving, pawn2.CurJob.GetTarget(TargetIndex.A), pawn2);
		}
	}
}
