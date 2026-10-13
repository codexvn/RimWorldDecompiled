using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020013A2 RID: 5026
	public static class Toils_Bed
	{
		// Token: 0x06007B41 RID: 31553 RVA: 0x002518E8 File Offset: 0x0024FAE8
		public static Toil GotoBed(TargetIndex bedIndex)
		{
			Toil gotoBed = ToilMaker.MakeToil("GotoBed");
			gotoBed.initAction = delegate()
			{
				Pawn actor = gotoBed.actor;
				Building_Bed bed = (Building_Bed)actor.CurJob.GetTarget(bedIndex).Thing;
				IntVec3 bedSleepingSlotPosFor = RestUtility.GetBedSleepingSlotPosFor(actor, bed);
				if (actor.Position == bedSleepingSlotPosFor)
				{
					actor.jobs.curDriver.ReadyForNextToil();
					return;
				}
				actor.pather.StartPath(bedSleepingSlotPosFor, PathEndMode.OnCell);
			};
			gotoBed.tickIntervalAction = delegate(int delta)
			{
				Pawn actor = gotoBed.actor;
				Building_Bed building_Bed = (Building_Bed)actor.CurJob.GetTarget(bedIndex).Thing;
				Pawn curOccupantAt = building_Bed.GetCurOccupantAt(actor.pather.Destination.Cell);
				if (curOccupantAt != null && curOccupantAt != actor)
				{
					actor.pather.StartPath(RestUtility.GetBedSleepingSlotPosFor(actor, building_Bed), PathEndMode.OnCell);
				}
			};
			gotoBed.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			gotoBed.FailOnBedNoLongerUsable(bedIndex);
			return gotoBed;
		}

		// Token: 0x06007B42 RID: 31554 RVA: 0x00251964 File Offset: 0x0024FB64
		public static Toil ClaimBedIfNonMedical(TargetIndex ind, TargetIndex claimantIndex = TargetIndex.None)
		{
			Toil claim = ToilMaker.MakeToil("ClaimBedIfNonMedical");
			claim.initAction = delegate()
			{
				Pawn actor = claim.GetActor();
				Pawn pawn = (claimantIndex == TargetIndex.None) ? actor : ((Pawn)actor.CurJob.GetTarget(claimantIndex).Thing);
				if (pawn.ownership != null && (!pawn.IsMutant || !pawn.mutant.Def.disableOwnership))
				{
					pawn.ownership.ClaimBedIfNonMedical((Building_Bed)actor.CurJob.GetTarget(ind).Thing);
				}
			};
			claim.FailOnDespawnedOrNull(ind);
			claim.defaultCompleteMode = ToilCompleteMode.Instant;
			claim.atomicWithPrevious = true;
			return claim;
		}

		// Token: 0x06007B43 RID: 31555 RVA: 0x002519DC File Offset: 0x0024FBDC
		public static bool BedNoLongerUsable(Pawn actor, Thing bedThing, bool forcePrisoner)
		{
			GuestStatus? guestStatusOverride = null;
			if (forcePrisoner)
			{
				guestStatusOverride = new GuestStatus?(GuestStatus.Prisoner);
			}
			return !RestUtility.CanUseBedNow(bedThing, actor, false, false, guestStatusOverride);
		}

		// Token: 0x06007B44 RID: 31556 RVA: 0x00251A0C File Offset: 0x0024FC0C
		private static Pawn TargetIndexToPawn(Toil toil, TargetIndex pawnIndex)
		{
			if (pawnIndex == TargetIndex.None)
			{
				return toil.actor;
			}
			return (Pawn)toil.actor.CurJob.GetTarget(pawnIndex).Thing;
		}

		// Token: 0x06007B45 RID: 31557 RVA: 0x00251A41 File Offset: 0x0024FC41
		public static void FailOnBedNoLongerUsable(this Toil toil, TargetIndex bedIndex)
		{
			toil.FailOnBedNoLongerUsable(bedIndex, TargetIndex.None);
		}

		// Token: 0x06007B46 RID: 31558 RVA: 0x00251A4C File Offset: 0x0024FC4C
		public static void FailOnBedNoLongerUsable(this Toil toil, TargetIndex bedIndex, TargetIndex sleeperIndex)
		{
			toil.FailOn(() => Toils_Bed.BedNoLongerUsable(Toils_Bed.TargetIndexToPawn(toil, sleeperIndex), toil.actor.CurJob.GetTarget(bedIndex).Thing, toil.actor.CurJobDef.makeTargetPrisoner));
			toil.AddFinishAction(delegate
			{
				Building_Bed building_Bed = (Building_Bed)toil.actor.CurJob.GetTarget(bedIndex).Thing;
				Pawn pawn = Toils_Bed.TargetIndexToPawn(toil, sleeperIndex);
				if (pawn.GetPosture().InBed() && Toils_Bed.BedNoLongerUsable(pawn, building_Bed, toil.actor.CurJobDef.makeTargetPrisoner))
				{
					RestUtility.KickOutOfBed(pawn, building_Bed);
				}
			});
		}

		// Token: 0x06007B47 RID: 31559 RVA: 0x00251AA4 File Offset: 0x0024FCA4
		public static Toil TuckIntoBed(Building_Bed bed, Pawn taker, Pawn takee, bool rescued = false)
		{
			Toil toil = ToilMaker.MakeToil("TuckIntoBed");
			toil.initAction = delegate()
			{
				RestUtility.TuckIntoBed(bed, taker, takee, rescued);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x06007B48 RID: 31560 RVA: 0x00251AF8 File Offset: 0x0024FCF8
		public static Toil TuckIntoBed(TargetIndex bedIndex, TargetIndex takeeIndex, bool rescued = false)
		{
			Toil tuckIntoBed = ToilMaker.MakeToil("TuckIntoBed");
			tuckIntoBed.initAction = delegate()
			{
				Pawn actor = tuckIntoBed.actor;
				Building_Bed building_Bed = actor.jobs.curJob.GetTarget(bedIndex).Thing as Building_Bed;
				Pawn takee = (Pawn)actor.jobs.curJob.GetTarget(takeeIndex).Thing;
				if (building_Bed != null)
				{
					RestUtility.TuckIntoBed(building_Bed, actor, takee, rescued);
					return;
				}
				Thing thing;
				actor.carryTracker.TryDropCarriedThing(actor.jobs.curJob.GetTarget(bedIndex).Cell, ThingPlaceMode.Direct, out thing, null);
			};
			tuckIntoBed.defaultCompleteMode = ToilCompleteMode.Instant;
			return tuckIntoBed;
		}
	}
}
