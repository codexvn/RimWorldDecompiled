using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000FD4 RID: 4052
	public static class ReservationUtility
	{
		// Token: 0x0600641B RID: 25627 RVA: 0x001FCABB File Offset: 0x001FACBB
		public static bool CanReserveSittableOrSpot(this Pawn pawn, IntVec3 exactSittingPos, bool ignoreOtherReservations = false)
		{
			return pawn.CanReserveSittableOrSpot(exactSittingPos, null, ignoreOtherReservations);
		}

		// Token: 0x0600641C RID: 25628 RVA: 0x001FCAC8 File Offset: 0x001FACC8
		public static bool CanReserveSittableOrSpot(this Pawn pawn, IntVec3 exactSittingPos, Thing ignoreThing, bool ignoreOtherReservations = false)
		{
			Building edifice = exactSittingPos.GetEdifice(pawn.Map);
			if (exactSittingPos.Impassable(pawn.Map) || exactSittingPos.IsForbidden(pawn))
			{
				return false;
			}
			for (int i = 0; i < 4; i++)
			{
				IntVec3 c = exactSittingPos + GenAdj.CardinalDirections[i];
				if (c.InBounds(pawn.Map))
				{
					Building edifice2 = c.GetEdifice(pawn.Map);
					Pawn pawn2;
					if (edifice2 != null && edifice2 != ignoreThing && edifice2.def.hasInteractionCell && edifice2.InteractionCell == exactSittingPos && pawn.Map.reservationManager.TryGetReserver(edifice2, pawn.Faction, out pawn2) && pawn2.Spawned && pawn2 != pawn)
					{
						return false;
					}
				}
			}
			if (edifice == null || edifice.def.building.multiSittable)
			{
				return pawn.CanReserve(exactSittingPos, 1, -1, null, ignoreOtherReservations);
			}
			return (!edifice.def.building.isSittable || !edifice.def.hasInteractionCell || !(exactSittingPos != edifice.InteractionCell)) && pawn.CanReserve(edifice, 1, -1, null, ignoreOtherReservations);
		}

		// Token: 0x0600641D RID: 25629 RVA: 0x001FCBF4 File Offset: 0x001FADF4
		public static bool ReserveSittableOrSpot(this Pawn pawn, IntVec3 exactSittingPos, Job job, bool errorOnFailed = true)
		{
			Building edifice = exactSittingPos.GetEdifice(pawn.Map);
			if (exactSittingPos.Impassable(pawn.Map))
			{
				Log.Error("Tried reserving impassable sittable or spot.");
				return false;
			}
			if (edifice == null || edifice.def.building.multiSittable)
			{
				return pawn.Reserve(exactSittingPos, job, 1, -1, null, errorOnFailed, false);
			}
			return (edifice == null || !edifice.def.building.isSittable || !edifice.def.hasInteractionCell || !(exactSittingPos != edifice.InteractionCell)) && pawn.Reserve(edifice, job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x0600641E RID: 25630 RVA: 0x001FCC94 File Offset: 0x001FAE94
		public static bool CanReserve(this Pawn p, LocalTargetInfo target, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null, bool ignoreOtherReservations = false)
		{
			return p.Spawned && p.Map.reservationManager.CanReserve(p, target, maxPawns, stackCount, layer, ignoreOtherReservations);
		}

		// Token: 0x0600641F RID: 25631 RVA: 0x001FCCB8 File Offset: 0x001FAEB8
		public static bool CanReserveAndReach(this Pawn p, LocalTargetInfo target, PathEndMode peMode, Danger maxDanger, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null, bool ignoreOtherReservations = false)
		{
			return p.Spawned && p.CanReach(target, peMode, maxDanger, false, false, TraverseMode.ByPawn) && p.Map.reservationManager.CanReserve(p, target, maxPawns, stackCount, layer, ignoreOtherReservations);
		}

		// Token: 0x06006420 RID: 25632 RVA: 0x001FCCF0 File Offset: 0x001FAEF0
		public static bool TryFindReserveAndReachableOfDef(this Pawn p, ThingDef thingDef, out Thing thing, PathEndMode peMode = PathEndMode.Touch, Danger maxDanger = Danger.None, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null, bool ignoreOtherReservations = false)
		{
			thing = null;
			if (!p.Spawned)
			{
				return false;
			}
			thing = GenClosest.ClosestThingReachable(p.PositionHeld, p.MapHeld, ThingRequest.ForDef(thingDef), PathEndMode.OnCell, TraverseParms.For(p, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing t) => !t.IsForbidden(p) && p.CanReserveAndReach(t, peMode, maxDanger, maxPawns, stackCount, layer, ignoreOtherReservations), null, 0, -1, false, RegionType.Set_Passable, false, false);
			return thing != null;
		}

		// Token: 0x06006421 RID: 25633 RVA: 0x001FCD9E File Offset: 0x001FAF9E
		public static bool HasReserved(this Pawn pawn, ThingDef thingDef)
		{
			return pawn.Spawned && pawn.MapHeld.reservationManager.HasReservedOfDef(pawn, thingDef);
		}

		// Token: 0x06006422 RID: 25634 RVA: 0x001FCDBC File Offset: 0x001FAFBC
		public static bool ExistsUnreservedAmountOfDef(Map map, ThingDef thingDef, Faction faction, int amount, Predicate<Thing> validator = null)
		{
			int num = 0;
			foreach (Thing thing in map.listerThings.ThingsOfDef(thingDef))
			{
				if (!thing.IsForbidden(faction) && !map.reservationManager.IsReservedByAnyoneOf(thing, faction) && (validator == null || validator(thing)))
				{
					num += thing.stackCount;
					if (num >= amount)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06006423 RID: 25635 RVA: 0x001FCE50 File Offset: 0x001FB050
		public static bool CanReserveAndReachableOfDef(this Pawn p, ThingDef thingDef, PathEndMode peMode = PathEndMode.Touch, Danger maxDanger = Danger.None, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null, bool ignoreOtherReservations = false)
		{
			Thing thing;
			return p.TryFindReserveAndReachableOfDef(thingDef, out thing, peMode, maxDanger, maxPawns, stackCount, layer, ignoreOtherReservations);
		}

		// Token: 0x06006424 RID: 25636 RVA: 0x001FCE70 File Offset: 0x001FB070
		public static bool Reserve(this Pawn p, LocalTargetInfo target, Job job, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null, bool errorOnFailed = true, bool ignoreOtherReservations = false)
		{
			return p.Spawned && p.Map.reservationManager.Reserve(p, job, target, maxPawns, stackCount, layer, errorOnFailed, ignoreOtherReservations, true);
		}

		// Token: 0x06006425 RID: 25637 RVA: 0x001FCEA4 File Offset: 0x001FB0A4
		public static void ReserveAsManyAsPossible(this Pawn p, List<LocalTargetInfo> target, Job job, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null)
		{
			if (!p.Spawned)
			{
				return;
			}
			for (int i = 0; i < target.Count; i++)
			{
				p.Map.reservationManager.Reserve(p, job, target[i], maxPawns, stackCount, layer, false, false, false);
			}
		}

		// Token: 0x06006426 RID: 25638 RVA: 0x001FCEED File Offset: 0x001FB0ED
		public static bool HasReserved(this Pawn p, LocalTargetInfo target, Job job = null)
		{
			return p.Spawned && p.Map.reservationManager.ReservedBy(target, p, job);
		}

		// Token: 0x06006427 RID: 25639 RVA: 0x001FCF0C File Offset: 0x001FB10C
		public static bool HasReserved<TDriver>(this Pawn p, LocalTargetInfo target, LocalTargetInfo? targetAIsNot = null, LocalTargetInfo? targetBIsNot = null, LocalTargetInfo? targetCIsNot = null)
		{
			return p.Spawned && p.Map.reservationManager.ReservedBy<TDriver>(target, p, targetAIsNot, targetBIsNot, targetCIsNot);
		}

		// Token: 0x06006428 RID: 25640 RVA: 0x001FCF2E File Offset: 0x001FB12E
		public static bool CanReserveNew(this Pawn p, LocalTargetInfo target)
		{
			return target.IsValid && !p.HasReserved(target, null) && p.CanReserve(target, 1, -1, null, false);
		}
	}
}
