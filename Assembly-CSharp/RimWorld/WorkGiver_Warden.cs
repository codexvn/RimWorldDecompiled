using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001584 RID: 5508
	public abstract class WorkGiver_Warden : WorkGiver_Scanner
	{
		// Token: 0x170013FF RID: 5119
		// (get) Token: 0x06008296 RID: 33430 RVA: 0x002735F9 File Offset: 0x002717F9
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x17001400 RID: 5120
		// (get) Token: 0x06008297 RID: 33431 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x06008298 RID: 33432 RVA: 0x00273704 File Offset: 0x00271904
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SlavesAndPrisonersOfColonySpawned;
		}

		// Token: 0x06008299 RID: 33433 RVA: 0x00273716 File Offset: 0x00271916
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.mapPawns.SlavesAndPrisonersOfColonySpawnedCount == 0;
		}

		// Token: 0x0600829A RID: 33434 RVA: 0x0027372C File Offset: 0x0027192C
		protected bool ShouldTakeCareOfPrisoner(Pawn warden, Thing prisoner, bool forced = false)
		{
			Pawn pawn = prisoner as Pawn;
			return pawn != null && pawn.IsPrisonerOfColony && pawn.guest.PrisonerIsSecure && pawn.Spawned && !pawn.InAggroMentalState && !prisoner.IsForbidden(warden) && !pawn.IsFormingCaravan() && warden.CanReserveAndReach(pawn, PathEndMode.OnCell, warden.NormalMaxDanger(), 1, -1, null, forced);
		}

		// Token: 0x0600829B RID: 33435 RVA: 0x00273798 File Offset: 0x00271998
		protected bool ShouldTakeCareOfSlave(Pawn warden, Thing slave, bool forced = false)
		{
			Pawn pawn = slave as Pawn;
			return pawn != null && pawn.IsSlaveOfColony && pawn.guest.SlaveIsSecure && pawn.Spawned && !pawn.InAggroMentalState && !pawn.IsForbidden(warden) && !pawn.IsFormingCaravan() && warden.CanReserveAndReach(pawn, PathEndMode.Touch, warden.NormalMaxDanger(), 1, -1, null, forced);
		}

		// Token: 0x0600829C RID: 33436 RVA: 0x00273804 File Offset: 0x00271A04
		protected bool IsExecutionIdeoAllowed(Pawn warden, Pawn victim)
		{
			return new HistoryEvent(HistoryEventDefOf.ExecutedPrisoner, warden.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job() && (!victim.guilt.IsGuilty || new HistoryEvent(HistoryEventDefOf.ExecutedPrisonerGuilty, warden.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job()) && (victim.guilt.IsGuilty || new HistoryEvent(HistoryEventDefOf.ExecutedPrisonerInnocent, warden.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job());
		}
	}
}
