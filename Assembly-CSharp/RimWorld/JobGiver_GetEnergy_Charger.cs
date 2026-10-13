using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001492 RID: 5266
	public class JobGiver_GetEnergy_Charger : JobGiver_GetEnergy
	{
		// Token: 0x06007E95 RID: 32405 RVA: 0x002616A0 File Offset: 0x0025F8A0
		public static Building_MechCharger GetClosestCharger(Pawn mech, Pawn carrier, bool forced)
		{
			if (!mech.Spawned || !carrier.Spawned)
			{
				return null;
			}
			Danger danger = forced ? Danger.Deadly : Danger.Some;
			return (Building_MechCharger)GenClosest.ClosestThingReachable(mech.Position, mech.Map, ThingRequest.ForGroup(ThingRequestGroup.MechCharger), PathEndMode.InteractionCell, TraverseParms.For(carrier, danger, TraverseMode.ByPawn, false, false, false, true), 9999f, delegate(Thing t)
			{
				Building_MechCharger building_MechCharger = (Building_MechCharger)t;
				if (!carrier.CanReach(t, PathEndMode.InteractionCell, danger, false, false, TraverseMode.ByPawn))
				{
					return false;
				}
				if (carrier != mech)
				{
					if (!forced && building_MechCharger.Map.reservationManager.ReservedBy(building_MechCharger, carrier, null))
					{
						return false;
					}
					if (forced && KeyBindingDefOf.QueueOrder.IsDownEvent && building_MechCharger.Map.reservationManager.ReservedBy(building_MechCharger, carrier, null))
					{
						return false;
					}
				}
				return !t.IsForbidden(carrier) && carrier.CanReserve(t, 1, -1, null, forced) && building_MechCharger.CanPawnChargeCurrently(mech);
			}, null, 0, -1, false, RegionType.Set_Passable, false, false);
		}

		// Token: 0x06007E96 RID: 32406 RVA: 0x0026174C File Offset: 0x0025F94C
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!this.ShouldAutoRecharge(pawn))
			{
				return null;
			}
			Building_MechCharger closestCharger = JobGiver_GetEnergy_Charger.GetClosestCharger(pawn, pawn, false);
			if (closestCharger != null)
			{
				Job job = JobMaker.MakeJob(JobDefOf.MechCharge, closestCharger);
				job.overrideFacing = Rot4.South;
				return job;
			}
			return null;
		}
	}
}
