using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001506 RID: 5382
	public class MeditationUtility
	{
		// Token: 0x06008031 RID: 32817 RVA: 0x00269F14 File Offset: 0x00268114
		public static Job GetMeditationJob(Pawn pawn, bool forJoy = false)
		{
			MeditationSpotAndFocus meditationSpotAndFocus = MeditationUtility.FindMeditationSpot(pawn);
			if (meditationSpotAndFocus.IsValid && pawn.CanReserveAndReach(meditationSpotAndFocus.spot, PathEndMode.OnCell, pawn.NormalMaxDanger(), 1, -1, null, false))
			{
				Building_Throne building_Throne = meditationSpotAndFocus.focus.Thing as Building_Throne;
				Job job;
				if (building_Throne != null)
				{
					job = JobMaker.MakeJob(JobDefOf.Reign, building_Throne, null, building_Throne);
				}
				else
				{
					JobDef def = JobDefOf.Meditate;
					if (forJoy && ModsConfig.IdeologyActive && pawn.Ideo != null)
					{
						IdeoFoundation_Deity ideoFoundation_Deity = pawn.Ideo.foundation as IdeoFoundation_Deity;
						if (ideoFoundation_Deity != null && ideoFoundation_Deity.DeitiesListForReading.Any<IdeoFoundation_Deity.Deity>())
						{
							def = JobDefOf.MeditatePray;
						}
					}
					job = JobMaker.MakeJob(def, meditationSpotAndFocus.spot, null, meditationSpotAndFocus.focus);
				}
				job.ignoreJoyTimeAssignment = !forJoy;
				return job;
			}
			return null;
		}

		// Token: 0x06008032 RID: 32818 RVA: 0x00269FEC File Offset: 0x002681EC
		public static MeditationSpotAndFocus FindMeditationSpot(Pawn pawn)
		{
			float num = float.MinValue;
			LocalTargetInfo spot = LocalTargetInfo.Invalid;
			LocalTargetInfo focus = LocalTargetInfo.Invalid;
			if (!ModLister.CheckRoyalty("Psyfocus"))
			{
				return new MeditationSpotAndFocus(spot, focus);
			}
			Room ownedRoom = pawn.ownership.OwnedRoom;
			foreach (LocalTargetInfo localTargetInfo in MeditationUtility.AllMeditationSpotCandidates(pawn, true))
			{
				if (MeditationUtility.SafeEnvironmentalConditions(pawn, localTargetInfo.Cell, pawn.Map) && localTargetInfo.Cell.Standable(pawn.Map) && !localTargetInfo.Cell.IsForbidden(pawn))
				{
					float num2 = 1f / Mathf.Max((float)localTargetInfo.Cell.DistanceToSquared(pawn.Position), 0.1f);
					LocalTargetInfo localTargetInfo2 = (localTargetInfo.Thing is Building_Throne) ? localTargetInfo.Thing : MeditationUtility.BestFocusAt(localTargetInfo, pawn);
					if (pawn.HasPsylink && localTargetInfo2.IsValid)
					{
						num2 += localTargetInfo2.Thing.GetStatValueForPawn(StatDefOf.MeditationFocusStrength, pawn, true) * 100f;
					}
					Room room = localTargetInfo.Cell.GetRoom(pawn.Map);
					if (room != null && ownedRoom == room)
					{
						num2 += 1f;
					}
					if (localTargetInfo.Thing != null)
					{
						Building building = localTargetInfo.Thing as Building;
						if (building != null && building.GetAssignedPawn() == pawn)
						{
							num2 += (float)((building.def == ThingDefOf.MeditationSpot) ? 200 : 100);
						}
					}
					if (room != null && ModsConfig.IdeologyActive && room.Role == RoomRoleDefOf.WorshipRoom)
					{
						num2 += 100f;
						foreach (Thing thing in room.ContainedAndAdjacentThings)
						{
							num2 += thing.GetStatValue(StatDefOf.StyleDominance, true, -1);
						}
					}
					if (num2 > num)
					{
						spot = localTargetInfo;
						focus = localTargetInfo2;
						num = num2;
					}
				}
			}
			return new MeditationSpotAndFocus(spot, focus);
		}

		// Token: 0x06008033 RID: 32819 RVA: 0x0026A238 File Offset: 0x00268438
		public static bool IsValidMeditationBuildingForPawn(Building spot, Pawn pawn)
		{
			if (spot.DestroyedOrNull() || spot.IsForbidden(pawn) || !spot.Position.Standable(spot.Map))
			{
				return false;
			}
			if (spot.GetAssignedPawn() != null && spot.GetAssignedPawn() != pawn)
			{
				return false;
			}
			Room room = spot.GetRoom(RegionType.Set_All);
			return (room == null || MeditationUtility.CanUseRoomToMeditate(room, pawn)) && (!(spot is Building_Throne) || RoomRoleWorker_ThroneRoom.Validate(room) == null) && pawn.CanReserveAndReach(spot, PathEndMode.OnCell, pawn.NormalMaxDanger(), 1, -1, null, false);
		}

		// Token: 0x06008034 RID: 32820 RVA: 0x0026A2BE File Offset: 0x002684BE
		public static IEnumerable<LocalTargetInfo> AllMeditationSpotCandidates(Pawn pawn, bool allowFallbackSpots = true)
		{
			bool flag = false;
			if (pawn.royalty != null && pawn.royalty.AllTitlesInEffectForReading.Count > 0 && !pawn.IsPrisonerOfColony)
			{
				Building_Throne building_Throne = RoyalTitleUtility.FindBestUsableThrone(pawn);
				if (building_Throne != null)
				{
					yield return building_Throne;
					flag = true;
				}
			}
			if (!pawn.IsPrisonerOfColony)
			{
				IEnumerable<Building> source = pawn.Map.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.MeditationSpot);
				Func<Building, bool> <>9__2;
				Func<Building, bool> predicate;
				if ((predicate = <>9__2) == null)
				{
					predicate = (<>9__2 = ((Building s) => MeditationUtility.IsValidMeditationBuildingForPawn(s, pawn)));
				}
				foreach (Building t in source.Where(predicate))
				{
					yield return t;
					flag = true;
				}
				IEnumerator<Building> enumerator = null;
			}
			if (flag || !allowFallbackSpots)
			{
				yield break;
			}
			List<Thing> list = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.MeditationFocus);
			foreach (Thing thing in list)
			{
				if (thing.def != ThingDefOf.Wall)
				{
					Room room = thing.GetRoom(RegionType.Set_All);
					if ((room == null || MeditationUtility.CanUseRoomToMeditate(room, pawn)) && thing.GetStatValueForPawn(StatDefOf.MeditationFocusStrength, pawn, true) > 0f)
					{
						LocalTargetInfo localTargetInfo = MeditationUtility.MeditationSpotForFocus(thing, pawn, null);
						if (localTargetInfo.IsValid)
						{
							yield return localTargetInfo;
						}
					}
				}
			}
			List<Thing>.Enumerator enumerator2 = default(List<Thing>.Enumerator);
			Building_Bed bed = pawn.ownership.OwnedBed;
			Building_Bed building_Bed = bed;
			Room room2 = (building_Bed != null) ? building_Bed.GetRoom(RegionType.Set_All) : null;
			IntVec3 c2;
			if (room2 != null && !room2.PsychologicallyOutdoors && pawn.CanReserveAndReach(bed, PathEndMode.OnCell, pawn.NormalMaxDanger(), 1, -1, null, false))
			{
				foreach (LocalTargetInfo localTargetInfo2 in MeditationUtility.FocusSpotsInTheRoom(pawn, room2))
				{
					yield return localTargetInfo2;
				}
				IEnumerator<LocalTargetInfo> enumerator3 = null;
				c2 = RCellFinder.RandomWanderDestFor(pawn, bed.Position, MeditationUtility.WanderRadius, (Pawn p, IntVec3 c, IntVec3 r) => c.Standable(p.Map) && c.GetDoor(p.Map) == null && WanderRoomUtility.IsValidWanderDest(p, c, r) && MeditationUtility.CanUseRoomToMeditate(c.GetRoom(p.Map), p), pawn.NormalMaxDanger(), false);
				if (c2.IsValid)
				{
					yield return c2;
				}
			}
			using (IEnumerator<Room> enumerator4 = MeditationUtility.UsableWorshipRooms(pawn).GetEnumerator())
			{
				while (enumerator4.MoveNext())
				{
					MeditationUtility.<>c__DisplayClass6_1 CS$<>8__locals2 = new MeditationUtility.<>c__DisplayClass6_1();
					CS$<>8__locals2.room = enumerator4.Current;
					foreach (LocalTargetInfo localTargetInfo3 in MeditationUtility.FocusSpotsInTheRoom(pawn, CS$<>8__locals2.room))
					{
						if (pawn.CanReach(localTargetInfo3, PathEndMode.Touch, pawn.NormalMaxDanger(), false, false, TraverseMode.ByPawn))
						{
							yield return localTargetInfo3;
						}
					}
					IEnumerator<LocalTargetInfo> enumerator3 = null;
					IntVec3 randomCell = CS$<>8__locals2.room.Districts.RandomElement<District>().Regions.RandomElement<Region>().RandomCell;
					c2 = RCellFinder.RandomWanderDestFor(pawn, randomCell, MeditationUtility.WanderRadius, (Pawn p, IntVec3 c, IntVec3 r) => c.GetRoom(p.Map) == CS$<>8__locals2.room && c.Standable(p.Map) && c.GetDoor(p.Map) == null && WanderRoomUtility.IsValidWanderDest(p, c, r), pawn.NormalMaxDanger(), false);
					if (c2.IsValid)
					{
						yield return c2;
					}
					CS$<>8__locals2 = null;
				}
			}
			IEnumerator<Room> enumerator4 = null;
			if (pawn.IsPrisonerOfColony)
			{
				yield break;
			}
			IntVec3 colonyWanderRoot = WanderUtility.GetColonyWanderRoot(pawn);
			c2 = RCellFinder.RandomWanderDestFor(pawn, colonyWanderRoot, MeditationUtility.WanderRadius, delegate(Pawn p, IntVec3 c, IntVec3 r)
			{
				if (!c.Standable(p.Map) || c.GetDoor(p.Map) != null || !p.CanReserveAndReach(c, PathEndMode.OnCell, p.NormalMaxDanger(), 1, -1, null, false))
				{
					return false;
				}
				Room room3 = c.GetRoom(p.Map);
				return room3 == null || MeditationUtility.CanUseRoomToMeditate(room3, pawn);
			}, pawn.NormalMaxDanger(), false);
			if (c2.IsValid)
			{
				yield return c2;
			}
			yield break;
			yield break;
		}

		// Token: 0x06008035 RID: 32821 RVA: 0x0026A2D5 File Offset: 0x002684D5
		public static IEnumerable<Room> UsableWorshipRooms(Pawn pawn)
		{
			foreach (Room room in pawn.Map.regionGrid.AllRooms)
			{
				if (room.Role == RoomRoleDefOf.WorshipRoom && MeditationUtility.CanUseRoomToMeditate(room, pawn))
				{
					yield return room;
				}
			}
			IEnumerator<Room> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06008036 RID: 32822 RVA: 0x0026A2E8 File Offset: 0x002684E8
		public static bool SafeEnvironmentalConditions(Pawn pawn, IntVec3 cell, Map map)
		{
			return (!map.gameConditionManager.ConditionIsActive(GameConditionDefOf.ToxicFallout) || cell.Roofed(map)) && (!ModsConfig.BiotechActive || !NoxiousHazeUtility.IsExposedToNoxiousHaze(pawn, cell, map)) && cell.GetDangerFor(pawn, map) == Danger.None && !cell.VacuumConcernTo(pawn);
		}

		// Token: 0x06008037 RID: 32823 RVA: 0x0026A340 File Offset: 0x00268540
		public static bool CanMeditateNow(Pawn pawn)
		{
			if (pawn.needs.rest != null && pawn.needs.rest.CurCategory >= RestCategory.VeryTired)
			{
				return false;
			}
			if (pawn.needs.food != null && pawn.needs.food.Starving)
			{
				return false;
			}
			if (pawn.Downed || !pawn.Awake())
			{
				return false;
			}
			if (pawn.health.hediffSet.BleedRateTotal <= 0f)
			{
				if (HealthAIUtility.ShouldSeekMedicalRest(pawn))
				{
					Pawn_TimetableTracker timetable = pawn.timetable;
					if (((timetable != null) ? timetable.CurrentAssignment : null) != TimeAssignmentDefOf.Meditate)
					{
						return false;
					}
				}
				if (!HealthAIUtility.ShouldSeekMedicalRestUrgent(pawn))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008038 RID: 32824 RVA: 0x0026A3E5 File Offset: 0x002685E5
		public static bool CanOnlyMeditateInBed(Pawn pawn)
		{
			return pawn.Downed;
		}

		// Token: 0x06008039 RID: 32825 RVA: 0x0026A3ED File Offset: 0x002685ED
		public static bool ShouldMeditateInBed(Pawn pawn)
		{
			return pawn.Downed || pawn.health.hediffSet.AnyHediffMakesSickThought;
		}

		// Token: 0x0600803A RID: 32826 RVA: 0x0026A410 File Offset: 0x00268610
		public static LocalTargetInfo BestFocusAt(LocalTargetInfo spot, Pawn pawn)
		{
			float num = 0f;
			LocalTargetInfo result = LocalTargetInfo.Invalid;
			foreach (Thing thing in GenRadial.RadialDistinctThingsAround(spot.Cell, pawn.MapHeld, MeditationUtility.FocusObjectSearchRadius, false))
			{
				if (GenSight.LineOfSightToThing(spot.Cell, thing, pawn.Map, false, null) && !(thing is Building_Throne))
				{
					CompMeditationFocus compMeditationFocus = thing.TryGetComp<CompMeditationFocus>();
					if (compMeditationFocus != null && compMeditationFocus.CanPawnUse(pawn))
					{
						float statValueForPawn = thing.GetStatValueForPawn(StatDefOf.MeditationFocusStrength, pawn, true);
						if (statValueForPawn > num)
						{
							result = thing;
							num = statValueForPawn;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600803B RID: 32827 RVA: 0x0026A4C8 File Offset: 0x002686C8
		public static IEnumerable<LocalTargetInfo> FocusSpotsInTheRoom(Pawn pawn, Room r)
		{
			List<Thing> things = r.ContainedAndAdjacentThings;
			int num;
			for (int i = 0; i < things.Count; i = num + 1)
			{
				CompMeditationFocus compMeditationFocus = things[i].TryGetComp<CompMeditationFocus>();
				if (compMeditationFocus != null && compMeditationFocus.CanPawnUse(pawn) && !(things[i] is Building_Throne) && things[i].GetStatValueForPawn(StatDefOf.MeditationFocusStrength, pawn, true) > 0f)
				{
					LocalTargetInfo localTargetInfo = MeditationUtility.MeditationSpotForFocus(things[i], pawn, new Func<IntVec3, bool>(r.ContainsCell));
					if (localTargetInfo.IsValid)
					{
						yield return localTargetInfo;
					}
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x0600803C RID: 32828 RVA: 0x0026A4E0 File Offset: 0x002686E0
		public static LocalTargetInfo MeditationSpotForFocus(Thing t, Pawn p, Func<IntVec3, bool> validator = null)
		{
			return (from cell in t.OccupiedRect().ExpandedBy(2).AdjacentCellsCardinal
			where cell.InBounds(t.Map) && (validator == null || validator(cell)) && !cell.IsForbidden(p) && p.CanReserveAndReach(cell, PathEndMode.OnCell, p.NormalMaxDanger(), 1, -1, null, false) && cell.Standable(p.Map)
			select cell).RandomElementWithFallback(IntVec3.Invalid);
		}

		// Token: 0x0600803D RID: 32829 RVA: 0x0026A544 File Offset: 0x00268744
		public static IEnumerable<MeditationFocusDef> FocusTypesAvailableForPawn(Pawn pawn)
		{
			int num;
			for (int i = 0; i < DefDatabase<MeditationFocusDef>.AllDefsListForReading.Count; i = num + 1)
			{
				MeditationFocusDef meditationFocusDef = DefDatabase<MeditationFocusDef>.AllDefsListForReading[i];
				if (meditationFocusDef.CanPawnUse(pawn))
				{
					yield return meditationFocusDef;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x0600803E RID: 32830 RVA: 0x0026A554 File Offset: 0x00268754
		public static string FocusTypesAvailableForPawnString(Pawn pawn)
		{
			return (from f in MeditationUtility.FocusTypesAvailableForPawn(pawn)
			select f.label).ToCommaList(false, false);
		}

		// Token: 0x0600803F RID: 32831 RVA: 0x0026A587 File Offset: 0x00268787
		public static IEnumerable<Dialog_InfoCard.Hyperlink> FocusObjectsForPawnHyperlinks(Pawn pawn)
		{
			int num;
			for (int i = 0; i < DefDatabase<MeditationFocusDef>.AllDefsListForReading.Count; i = num + 1)
			{
				MeditationFocusDef meditationFocusDef = DefDatabase<MeditationFocusDef>.AllDefsListForReading[i];
				if (meditationFocusDef.CanPawnUse(pawn))
				{
					if (!MeditationUtility.focusObjectHyperlinksPerTypeCache.ContainsKey(meditationFocusDef))
					{
						List<Dialog_InfoCard.Hyperlink> list2 = new List<Dialog_InfoCard.Hyperlink>();
						foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefsListForReading)
						{
							CompProperties_MeditationFocus compProperties = thingDef.GetCompProperties<CompProperties_MeditationFocus>();
							if (compProperties != null && compProperties.focusTypes.Contains(meditationFocusDef))
							{
								list2.Add(new Dialog_InfoCard.Hyperlink(thingDef, -1));
							}
						}
						MeditationUtility.focusObjectHyperlinksPerTypeCache[meditationFocusDef] = list2;
					}
					List<Dialog_InfoCard.Hyperlink> list = MeditationUtility.focusObjectHyperlinksPerTypeCache[meditationFocusDef];
					for (int j = 0; j < list.Count; j = num + 1)
					{
						yield return list[j];
						num = j;
					}
					list = null;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06008040 RID: 32832 RVA: 0x0026A598 File Offset: 0x00268798
		public static string FocusTypeAvailableExplanation(Pawn pawn)
		{
			string text = "";
			for (int i = 0; i < DefDatabase<MeditationFocusDef>.AllDefsListForReading.Count; i++)
			{
				MeditationFocusDef meditationFocusDef = DefDatabase<MeditationFocusDef>.AllDefsListForReading[i];
				if (meditationFocusDef.CanPawnUse(pawn))
				{
					text = string.Concat(new string[]
					{
						text,
						"MeditationFocusCanUse".Translate(meditationFocusDef.label).Resolve(),
						":\n",
						meditationFocusDef.EnablingThingsExplanation(pawn),
						"\n\n"
					});
					if (!MeditationUtility.focusObjectsPerTypeCache.ContainsKey(meditationFocusDef))
					{
						List<string> list = new List<string>();
						foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefsListForReading)
						{
							CompProperties_MeditationFocus compProperties = thingDef.GetCompProperties<CompProperties_MeditationFocus>();
							if (compProperties != null && compProperties.focusTypes.Contains(meditationFocusDef))
							{
								list.AddUnique(thingDef.label);
							}
						}
						MeditationUtility.focusObjectsPerTypeCache[meditationFocusDef] = list.ToLineList("  - ", true);
					}
					text += "MeditationFocusObjects".Translate(meditationFocusDef.label).CapitalizeFirst() + ":\n" + MeditationUtility.focusObjectsPerTypeCache[meditationFocusDef] + "\n\n";
				}
			}
			return text;
		}

		// Token: 0x06008041 RID: 32833 RVA: 0x0026A70C File Offset: 0x0026890C
		public static void DrawMeditationSpotOverlay(IntVec3 center, Map map)
		{
			GenDraw.DrawRadiusRing(center, MeditationUtility.FocusObjectSearchRadius);
			foreach (Thing thing in GenRadial.RadialDistinctThingsAround(center, map, MeditationUtility.FocusObjectSearchRadius, false))
			{
				if (!(thing is Building_Throne) && (thing.def.building == null || !thing.def.building.supportsWallAttachments) && thing.TryGetComp<CompMeditationFocus>() != null && GenSight.LineOfSight(center, thing.Position, map))
				{
					GenDraw.DrawLineBetween(center.ToVector3() + new Vector3(0.5f, 0f, 0.5f), thing.TrueCenter(), SimpleColor.White, 0.2f);
				}
			}
		}

		// Token: 0x06008042 RID: 32834 RVA: 0x0026A7D4 File Offset: 0x002689D4
		public static bool CanUseRoomToMeditate(Room r, Pawn p)
		{
			return (r.Owners.EnumerableNullOrEmpty<Pawn>() || r.Owners.Contains(p)) && (!r.IsPrisonCell || p.IsPrisoner);
		}

		// Token: 0x06008043 RID: 32835 RVA: 0x0026A806 File Offset: 0x00268A06
		public static IEnumerable<Thing> GetMeditationFociAffectedByBuilding(Map map, ThingDef def, Faction faction, IntVec3 pos, Rot4 rotation)
		{
			if (!MeditationUtility.CountsAsArtificialBuilding(def, faction))
			{
				yield break;
			}
			foreach (Thing thing in map.listerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.MeditationFocus)))
			{
				CompMeditationFocus compMeditationFocus = thing.TryGetComp<CompMeditationFocus>();
				if (compMeditationFocus != null && compMeditationFocus.WillBeAffectedBy(def, faction, pos, rotation))
				{
					yield return thing;
				}
			}
			List<Thing>.Enumerator enumerator = default(List<Thing>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06008044 RID: 32836 RVA: 0x0026A834 File Offset: 0x00268A34
		public static void DrawMeditationFociAffectedByBuildingOverlay(Map map, ThingDef def, Faction faction, IntVec3 pos, Rot4 rotation)
		{
			int num = 0;
			foreach (Thing thing in MeditationUtility.GetMeditationFociAffectedByBuilding(map, def, faction, pos, rotation))
			{
				if (num++ > 10)
				{
					break;
				}
				CompToggleDrawAffectedMeditationFoci compToggleDrawAffectedMeditationFoci = thing.TryGetComp<CompToggleDrawAffectedMeditationFoci>();
				if (compToggleDrawAffectedMeditationFoci == null || compToggleDrawAffectedMeditationFoci.Enabled)
				{
					GenAdj.OccupiedRect(pos, rotation, def.size);
					GenDraw.DrawLineBetween(GenThing.TrueCenter(pos, rotation, def.size, def.Altitude), thing.TrueCenter(), SimpleColor.Red, 0.2f);
				}
			}
		}

		// Token: 0x06008045 RID: 32837 RVA: 0x0026A8D4 File Offset: 0x00268AD4
		public static bool CountsAsArtificialBuilding(ThingDef def, Faction faction)
		{
			return (def.category == ThingCategory.Building || def.thingCategories.NotNullAndContains(ThingCategoryDefOf.BuildingsSpecial)) && faction != null && def.building.artificialForMeditationPurposes;
		}

		// Token: 0x06008046 RID: 32838 RVA: 0x0026A901 File Offset: 0x00268B01
		public static bool CountsAsArtificialBuilding(Thing t)
		{
			return MeditationUtility.CountsAsArtificialBuilding(t.def, t.Faction);
		}

		// Token: 0x06008047 RID: 32839 RVA: 0x0026A914 File Offset: 0x00268B14
		public static void DrawArtificialBuildingOverlay(IntVec3 pos, ThingDef def, Map map, float radius)
		{
			GenDraw.DrawRadiusRing(pos, radius, MeditationUtility.ArtificialBuildingRingColor, null);
			int num = 0;
			foreach (Thing t in map.listerArtificialBuildingsForMeditation.GetForCell(pos, radius))
			{
				if (num++ > 10)
				{
					break;
				}
				GenDraw.DrawLineBetween(GenThing.TrueCenter(pos, Rot4.North, def.size, def.Altitude), t.TrueCenter(), SimpleColor.Red, 0.2f);
			}
		}

		// Token: 0x06008048 RID: 32840 RVA: 0x0026A9AC File Offset: 0x00268BAC
		public static float PsyfocusGainPerTick(Pawn pawn, Thing focus = null)
		{
			float num = pawn.GetStatValue(StatDefOf.MeditationFocusGain, true, -1);
			if (focus != null && !focus.Destroyed)
			{
				num += focus.GetStatValueForPawn(StatDefOf.MeditationFocusStrength, pawn, true);
			}
			return num / 60000f;
		}

		// Token: 0x06008049 RID: 32841 RVA: 0x0026A9EC File Offset: 0x00268BEC
		public static void CheckMeditationScheduleTeachOpportunity(Pawn pawn)
		{
			if (pawn.Dead || !pawn.Spawned || !pawn.HasPsylink)
			{
				return;
			}
			if (pawn.Faction != Faction.OfPlayer || pawn.IsQuestLodger())
			{
				return;
			}
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.MeditationSchedule, pawn, OpportunityType.GoodToKnow);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.MeditationDesiredPsyfocus, pawn, OpportunityType.GoodToKnow);
		}

		// Token: 0x040055AE RID: 21934
		public static float FocusObjectSearchRadius = 3.9f;

		// Token: 0x040055AF RID: 21935
		private static float WanderRadius = 10f;

		// Token: 0x040055B0 RID: 21936
		public static readonly Color ArtificialBuildingRingColor = new Color(0.8f, 0.49f, 0.43f);

		// Token: 0x040055B1 RID: 21937
		private static Dictionary<MeditationFocusDef, List<Dialog_InfoCard.Hyperlink>> focusObjectHyperlinksPerTypeCache = new Dictionary<MeditationFocusDef, List<Dialog_InfoCard.Hyperlink>>();

		// Token: 0x040055B2 RID: 21938
		private static Dictionary<MeditationFocusDef, string> focusObjectsPerTypeCache = new Dictionary<MeditationFocusDef, string>();
	}
}
