using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001539 RID: 5433
	public class BabyPlayGiver_PlayToys : BabyPlayGiver
	{
		// Token: 0x0600810B RID: 33035 RVA: 0x0026D990 File Offset: 0x0026BB90
		public override bool CanDo(Pawn pawn, Pawn baby)
		{
			Thing thing = this.FindNearbyUseableToyBox(pawn, baby);
			return thing != null && (pawn.IsCarryingPawn(baby) || pawn.CanReserveAndReach(baby, PathEndMode.Touch, Danger.Some, 1, -1, null, false)) && pawn.CanReserveAndReach(thing, PathEndMode.Touch, Danger.Some, 1, -1, null, false);
		}

		// Token: 0x0600810C RID: 33036 RVA: 0x0026D9E4 File Offset: 0x0026BBE4
		public override Job TryGiveJob(Pawn pawn, Pawn baby)
		{
			Thing thing = this.FindNearbyUseableToyBox(pawn, baby);
			if (thing != null)
			{
				Job job = JobMaker.MakeJob(this.def.jobDef, baby, thing);
				job.count = 1;
				return job;
			}
			return null;
		}

		// Token: 0x0600810D RID: 33037 RVA: 0x0026DA24 File Offset: 0x0026BC24
		private bool IsValidToybox(Thing toybox, Pawn hauler, Pawn baby)
		{
			return toybox.def == ThingDefOf.ToyBox && !toybox.IsForbidden(hauler) && !toybox.IsForbidden(baby) && !toybox.IsBurning() && hauler.CanReserveAndReach(toybox, PathEndMode.Touch, Danger.None, 1, -1, null, false) && hauler.Position.DistanceTo(toybox.Position) <= 15.9f;
		}

		// Token: 0x0600810E RID: 33038 RVA: 0x0026DA90 File Offset: 0x0026BC90
		private Thing FindNearbyUseableToyBox(Pawn pawn, Pawn baby)
		{
			Room room = baby.GetRoom(RegionType.Set_All);
			if (room != null)
			{
				foreach (Thing thing in room.ContainedThings(ThingDefOf.ToyBox))
				{
					if (this.IsValidToybox(thing, pawn, baby))
					{
						return thing;
					}
				}
			}
			return GenClosest.ClosestThingReachable(baby.PositionHeld, baby.MapHeld, ThingRequest.ForDef(ThingDefOf.ToyBox), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 15.9f, (Thing t) => this.IsValidToybox(t, pawn, baby), null, 0, -1, false, RegionType.Set_Passable, false, false);
		}

		// Token: 0x04005620 RID: 22048
		private const float MaxToyBoxDistance = 15.9f;
	}
}
