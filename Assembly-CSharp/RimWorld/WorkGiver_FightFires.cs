using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015C5 RID: 5573
	internal class WorkGiver_FightFires : WorkGiver_Scanner
	{
		// Token: 0x1700142A RID: 5162
		// (get) Token: 0x060083BA RID: 33722 RVA: 0x002783BF File Offset: 0x002765BF
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.Fire);
			}
		}

		// Token: 0x1700142B RID: 5163
		// (get) Token: 0x060083BB RID: 33723 RVA: 0x000E96BB File Offset: 0x000E78BB
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060083BC RID: 33724 RVA: 0x00075162 File Offset: 0x00073362
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x060083BD RID: 33725 RVA: 0x002783CC File Offset: 0x002765CC
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Fire fire = t as Fire;
			if (fire == null)
			{
				return false;
			}
			Pawn pawn2 = fire.parent as Pawn;
			if (pawn2 != null)
			{
				if (pawn2 == pawn)
				{
					return false;
				}
				if ((pawn2.Faction == null || pawn2.Faction != pawn.Faction) && (pawn2.HostFaction == null || (pawn2.HostFaction != pawn.Faction && pawn2.HostFaction != pawn.HostFaction)))
				{
					return false;
				}
				if (!pawn.Map.areaManager.Home[fire.Position] && IntVec3Utility.ManhattanDistanceFlat(pawn.Position, pawn2.Position) > 15)
				{
					return false;
				}
				if (!pawn.CanReach(pawn2, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					return false;
				}
			}
			else
			{
				if (pawn.WorkTagIsDisabled(WorkTags.Firefighting))
				{
					JobFailReason.Is("IncapableOfFirefighting".Translate(), null);
					return false;
				}
				if (!pawn.Map.areaManager.Home[fire.Position])
				{
					JobFailReason.Is(WorkGiver_FixBrokenDownBuilding.NotInHomeAreaTrans, null);
					return false;
				}
			}
			return ((pawn.Position - fire.Position).LengthHorizontalSquared <= 225 || pawn.CanReserve(fire, 1, -1, null, forced)) && (forced || !WorkGiver_FightFires.FireIsBeingHandled(fire, pawn));
		}

		// Token: 0x060083BE RID: 33726 RVA: 0x0027851E File Offset: 0x0027671E
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.BeatFire, t);
		}

		// Token: 0x060083BF RID: 33727 RVA: 0x00278530 File Offset: 0x00276730
		public static bool FireIsBeingHandled(Fire f, Pawn potentialHandler)
		{
			if (!f.Spawned)
			{
				return false;
			}
			Pawn pawn = f.Map.reservationManager.FirstRespectedReserver(f, potentialHandler, null);
			return pawn != null && pawn.Position.InHorDistOf(f.Position, 5f);
		}

		// Token: 0x040056F0 RID: 22256
		public const int NearbyPawnRadius = 15;

		// Token: 0x040056F1 RID: 22257
		private const int MaxReservationCheckDistance = 15;

		// Token: 0x040056F2 RID: 22258
		private const float HandledDistance = 5f;
	}
}
