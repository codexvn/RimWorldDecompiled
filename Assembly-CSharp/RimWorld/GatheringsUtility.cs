using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200162C RID: 5676
	public static class GatheringsUtility
	{
		// Token: 0x060085D4 RID: 34260 RVA: 0x0027F060 File Offset: 0x0027D260
		public static bool ShouldGuestKeepAttendingGathering(Pawn p)
		{
			if (p.Downed)
			{
				return false;
			}
			Pawn_NeedsTracker needs = p.needs;
			if (((needs != null) ? needs.food : null) != null && p.needs.food.Starving)
			{
				return false;
			}
			if (p.health.hediffSet.BleedRateTotal > 0f)
			{
				return false;
			}
			Pawn_NeedsTracker needs2 = p.needs;
			return (((needs2 != null) ? needs2.rest : null) == null || p.needs.rest.CurCategory < RestCategory.Exhausted) && !p.health.hediffSet.HasTendableNonInjuryNonMissingPartHediff(false) && !p.health.hediffSet.InLabor(true) && p.Awake() && !p.InAggroMentalState && !p.IsPrisoner && !p.IsSlave && !p.Inhumanized();
		}

		// Token: 0x060085D5 RID: 34261 RVA: 0x0027F13C File Offset: 0x0027D33C
		public static bool ShouldGuestKeepAttendingRitual(Pawn p, bool ignoreBleeding = false)
		{
			return !p.Downed && !p.health.hediffSet.HasTemperatureInjury(TemperatureInjuryStage.Serious) && (ignoreBleeding || p.health.hediffSet.BleedRateTotal <= 0f) && !p.InAggroMentalState;
		}

		// Token: 0x060085D6 RID: 34262 RVA: 0x0027F190 File Offset: 0x0027D390
		public static bool PawnCanStartOrContinueGathering(Pawn pawn)
		{
			if (pawn.Drafted)
			{
				return false;
			}
			if (pawn.health.hediffSet.BleedRateTotal > 0.3f)
			{
				return false;
			}
			if (pawn.IsPrisoner || pawn.IsSlave)
			{
				return false;
			}
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BloodLoss, false);
			return (firstHediffOfDef == null || firstHediffOfDef.Severity <= 0.2f) && !pawn.IsWildMan() && !pawn.Inhumanized() && !pawn.IsSubhuman && (pawn.Spawned && !pawn.Downed) && !pawn.InMentalState;
		}

		// Token: 0x060085D7 RID: 34263 RVA: 0x0027F234 File Offset: 0x0027D434
		public static bool AnyLordJobPreventsNewGatherings(Map map)
		{
			List<Lord> lords = map.lordManager.lords;
			for (int i = 0; i < lords.Count; i++)
			{
				if (lords[i].ownedPawns.Count != 0 && !lords[i].LordJob.AllowStartNewGatherings)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060085D8 RID: 34264 RVA: 0x0027F288 File Offset: 0x0027D488
		public static bool AnyLordJobPreventsNewRituals(Map map)
		{
			List<Lord> lords = map.lordManager.lords;
			for (int i = 0; i < lords.Count; i++)
			{
				LordJob lordJob = lords[i].LordJob;
				if (lords[i].ownedPawns.Count != 0 && !lordJob.AllowStartNewRituals)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060085D9 RID: 34265 RVA: 0x0027F2E0 File Offset: 0x0027D4E0
		public static bool AcceptableGameConditionsToStartGathering(Map map, GatheringDef gatheringDef)
		{
			if (!GatheringsUtility.AcceptableGameConditionsToContinueGathering(map))
			{
				return false;
			}
			if (GenLocalDate.HourInteger(map) < 4 || GenLocalDate.HourInteger(map) > 21)
			{
				return false;
			}
			if (GatheringsUtility.AnyLordJobPreventsNewGatherings(map))
			{
				return false;
			}
			if (map.dangerWatcher.DangerRating != StoryDanger.None)
			{
				return false;
			}
			int freeColonistsSpawnedCount = map.mapPawns.FreeColonistsSpawnedCount;
			if (freeColonistsSpawnedCount < 4)
			{
				return false;
			}
			int num = 0;
			foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
			{
				if (pawn.health.hediffSet.BleedRateTotal > 0f)
				{
					return false;
				}
				if (pawn.Drafted)
				{
					num++;
				}
			}
			return (float)num / (float)freeColonistsSpawnedCount < 0.5f && GatheringsUtility.EnoughPotentialGuestsToStartGathering(map, gatheringDef, null);
		}

		// Token: 0x060085DA RID: 34266 RVA: 0x0027F3CC File Offset: 0x0027D5CC
		public static bool AcceptableGameConditionsToContinueGathering(Map map)
		{
			return map.dangerWatcher.DangerRating != StoryDanger.High;
		}

		// Token: 0x060085DB RID: 34267 RVA: 0x0027F3E0 File Offset: 0x0027D5E0
		public static bool ValidateGatheringSpot(IntVec3 cell, GatheringDef gatheringDef, Pawn organizer, bool enjoyableOutside, bool ignoreRequiredColonistCount)
		{
			Map map = organizer.Map;
			if (!cell.Standable(map))
			{
				return false;
			}
			if (cell.GetDangerFor(organizer, map) != Danger.None)
			{
				return false;
			}
			if (cell.GetTerrain(map).dangerous)
			{
				return false;
			}
			if (!enjoyableOutside && !cell.Roofed(map))
			{
				return false;
			}
			if (cell.IsForbidden(organizer))
			{
				return false;
			}
			if (!organizer.CanReserveAndReach(cell, PathEndMode.OnCell, Danger.None, 1, -1, null, false))
			{
				return false;
			}
			Room room = cell.GetRoom(map);
			bool flag = room != null && room.IsPrisonCell;
			return organizer.IsPrisoner == flag && (ignoreRequiredColonistCount || GatheringsUtility.EnoughPotentialGuestsToStartGathering(map, gatheringDef, new IntVec3?(cell)));
		}

		// Token: 0x060085DC RID: 34268 RVA: 0x0027F484 File Offset: 0x0027D684
		public static bool EnoughPotentialGuestsToStartGathering(Map map, GatheringDef gatheringDef, IntVec3? gatherSpot = null)
		{
			int num = Mathf.RoundToInt((float)map.mapPawns.FreeColonistsSpawnedCount * 0.65f);
			num = Mathf.Clamp(num, 2, 10);
			int num2 = 0;
			foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
			{
				if (GatheringsUtility.ShouldPawnKeepGathering(pawn, gatheringDef) && (gatherSpot == null || !gatherSpot.Value.IsForbidden(pawn)) && (gatherSpot == null || pawn.CanReach(gatherSpot.Value, PathEndMode.Touch, Danger.Some, false, false, TraverseMode.ByPawn)))
				{
					num2++;
				}
			}
			return num2 >= num;
		}

		// Token: 0x060085DD RID: 34269 RVA: 0x0027F548 File Offset: 0x0027D748
		public static Pawn FindRandomGatheringOrganizer(Faction faction, Map map, GatheringDef gatheringDef)
		{
			GatheringsUtility.<>c__DisplayClass12_0 CS$<>8__locals1 = new GatheringsUtility.<>c__DisplayClass12_0();
			CS$<>8__locals1.gatheringDef = gatheringDef;
			Pawn result;
			if (map.mapPawns.SpawnedPawnsInFaction(faction).Where(new Func<Pawn, bool>(CS$<>8__locals1.<FindRandomGatheringOrganizer>g__CanOrganize|0)).TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x060085DE RID: 34270 RVA: 0x0027F58C File Offset: 0x0027D78C
		public static bool InGatheringArea(IntVec3 cell, IntVec3 partySpot, Map map)
		{
			GatheringsUtility.<>c__DisplayClass13_0 CS$<>8__locals1;
			CS$<>8__locals1.cell = cell;
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.partySpot = partySpot;
			if (GatheringsUtility.UseWholeRoomAsGatheringArea(CS$<>8__locals1.partySpot, CS$<>8__locals1.map))
			{
				return CS$<>8__locals1.cell.GetRoom(CS$<>8__locals1.map) == CS$<>8__locals1.partySpot.GetRoom(CS$<>8__locals1.map);
			}
			if (!CS$<>8__locals1.cell.InHorDistOf(CS$<>8__locals1.partySpot, 18f))
			{
				return false;
			}
			Building edifice = CS$<>8__locals1.cell.GetEdifice(CS$<>8__locals1.map);
			TraverseParms traverseParams = TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.None, false, false, false, true, false);
			if (edifice != null)
			{
				return CS$<>8__locals1.map.reachability.CanReach(CS$<>8__locals1.partySpot, edifice, PathEndMode.ClosestTouch, traverseParams) && !GatheringsUtility.<InGatheringArea>g__NeedsToPassDoor|13_0(ref CS$<>8__locals1);
			}
			return CS$<>8__locals1.map.reachability.CanReach(CS$<>8__locals1.partySpot, CS$<>8__locals1.cell, PathEndMode.ClosestTouch, traverseParams) && !GatheringsUtility.<InGatheringArea>g__NeedsToPassDoor|13_0(ref CS$<>8__locals1);
		}

		// Token: 0x060085DF RID: 34271 RVA: 0x0027F688 File Offset: 0x0027D888
		public static bool TryFindRandomCellInGatheringArea(Pawn pawn, Predicate<IntVec3> cellValidator, out IntVec3 result)
		{
			IntVec3 cell = pawn.mindState.duty.focus.Cell;
			Predicate<IntVec3> validator = (IntVec3 x) => x.Standable(pawn.Map) && !x.IsForbidden(pawn) && pawn.CanReserveAndReach(x, PathEndMode.OnCell, Danger.None, 1, -1, null, false) && (cellValidator == null || cellValidator(x));
			if (GatheringsUtility.UseWholeRoomAsGatheringArea(cell, pawn.Map))
			{
				return (from x in cell.GetRoom(pawn.Map).Cells
				where validator(x)
				select x).TryRandomElement(out result);
			}
			return CellFinder.TryFindRandomReachableNearbyCell(cell, pawn.Map, 8f, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false, true, false), (IntVec3 x) => validator(x), null, out result, 10);
		}

		// Token: 0x060085E0 RID: 34272 RVA: 0x0027F74C File Offset: 0x0027D94C
		public static bool TryFindRandomCellInGatheringAreaWithRadius(Pawn pawn, float radius, Predicate<IntVec3> cellValidator, out IntVec3 result)
		{
			IntVec3 partySpot = pawn.mindState.duty.focus.Cell;
			Predicate<IntVec3> validator = (IntVec3 x) => x.Standable(pawn.Map) && !x.IsForbidden(pawn) && pawn.CanReserveAndReach(x, PathEndMode.OnCell, Danger.None, 1, -1, null, false) && (cellValidator == null || cellValidator(x));
			if (GatheringsUtility.UseWholeRoomAsGatheringArea(partySpot, pawn.Map))
			{
				return (from x in partySpot.GetRoom(pawn.Map).Cells
				where validator(x) && x.InHorDistOf(partySpot, radius)
				select x).TryRandomElement(out result);
			}
			return CellFinder.TryFindRandomReachableNearbyCell(partySpot, pawn.Map, radius, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false, true, false), (IntVec3 x) => validator(x), null, out result, 10);
		}

		// Token: 0x060085E1 RID: 34273 RVA: 0x0027F82C File Offset: 0x0027DA2C
		public static bool TryFindRandomCellAroundTarget(Pawn pawn, Thing target, out IntVec3 result)
		{
			if (!(from c in target.OccupiedRect().ExpandedBy(1).EdgeCells
			where c.InBounds(pawn.Map) && c.Standable(pawn.Map) && !c.IsForbidden(pawn) && pawn.CanReserveSittableOrSpot(c, false)
			select c).TryRandomElement(out result))
			{
				result = IntVec3.Invalid;
				return false;
			}
			return true;
		}

		// Token: 0x060085E2 RID: 34274 RVA: 0x0027F884 File Offset: 0x0027DA84
		public static bool UseWholeRoomAsGatheringArea(IntVec3 partySpot, Map map)
		{
			Room room = partySpot.GetRoom(map);
			return room != null && !room.IsHuge && room.CellCount <= 100 && !room.PsychologicallyOutdoors;
		}

		// Token: 0x060085E3 RID: 34275 RVA: 0x0027F8B9 File Offset: 0x0027DAB9
		public static bool ShouldPawnKeepGathering(Pawn p, GatheringDef gatheringDef)
		{
			return (gatheringDef == null || !gatheringDef.respectTimetable || p.timetable == null || p.timetable.CurrentAssignment.allowJoy) && GatheringsUtility.ShouldGuestKeepAttendingGathering(p);
		}

		// Token: 0x060085E4 RID: 34276 RVA: 0x0027F8ED File Offset: 0x0027DAED
		public static bool ShouldPawnKeepAttendingRitual(Pawn p, Precept_Ritual ritual, bool ignoreBleeding = false)
		{
			return GatheringsUtility.ShouldGuestKeepAttendingRitual(p, ignoreBleeding);
		}

		// Token: 0x060085E5 RID: 34277 RVA: 0x0027F8FB File Offset: 0x0027DAFB
		[CompilerGenerated]
		internal static bool <InGatheringArea>g__NeedsToPassDoor|13_0(ref GatheringsUtility.<>c__DisplayClass13_0 A_0)
		{
			return A_0.cell.GetRoom(A_0.map) != A_0.partySpot.GetRoom(A_0.map);
		}

		// Token: 0x0400576F RID: 22383
		public const float GatherAreaRadiusIfNotWholeRoom = 18f;

		// Token: 0x04005770 RID: 22384
		public const float WanderAreaRadiusIfNotWholeRoom = 8f;

		// Token: 0x04005771 RID: 22385
		private const int MaxRoomCellsCountToUseWholeRoom = 100;
	}
}
