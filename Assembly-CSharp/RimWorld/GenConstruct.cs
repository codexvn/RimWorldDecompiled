using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02003642 RID: 13890
	public static class GenConstruct
	{
		// Token: 0x0601348B RID: 78987 RVA: 0x005B8B6C File Offset: 0x005B6D6C
		public static void Reset()
		{
			GenConstruct.SkillTooLowTrans = "SkillTooLowForConstruction".Translate();
			GenConstruct.IncapableOfDeconstruction = "IncapableOfDeconstruction".Translate();
			GenConstruct.IncapableOfMining = "IncapableOfMining".Translate();
			GenConstruct.TreeMarkedForExtraction = "TreeMarkedForExtraction".Translate();
		}

		// Token: 0x0601348C RID: 78988 RVA: 0x005B8BCC File Offset: 0x005B6DCC
		public static Blueprint_Build PlaceBlueprintForBuild(BuildableDef sourceDef, IntVec3 center, Map map, Rot4 rotation, Faction faction, ThingDef stuff, Precept_ThingStyle styleSource = null, ThingStyleDef styleDef = null, bool sendBPSpawnedSignal = true)
		{
			Blueprint_Build blueprint_Build = (Blueprint_Build)ThingMaker.MakeThing(sourceDef.blueprintDef, null);
			blueprint_Build.SetFactionDirect(faction);
			blueprint_Build.stuffToUse = stuff;
			blueprint_Build.InheritStyle(styleSource, styleDef);
			GenSpawn.Spawn(blueprint_Build, center, map, rotation, WipeMode.Vanish, false, false);
			if (faction != null && sendBPSpawnedSignal)
			{
				QuestUtility.SendQuestTargetSignals(faction.questTags, "PlacedBlueprint", blueprint_Build.Named("SUBJECT"));
			}
			return blueprint_Build;
		}

		// Token: 0x0601348D RID: 78989 RVA: 0x005B8C3C File Offset: 0x005B6E3C
		public static Blueprint_Install PlaceBlueprintForInstall(MinifiedThing itemToInstall, IntVec3 center, Map map, Rot4 rotation, Faction faction, bool sendBPSpawnedSignal = true)
		{
			Blueprint_Install blueprint_Install = (Blueprint_Install)ThingMaker.MakeThing(itemToInstall.InnerThing.def.installBlueprintDef, null);
			blueprint_Install.SetThingToInstallFromMinified(itemToInstall);
			blueprint_Install.SetFactionDirect(faction);
			GenSpawn.Spawn(blueprint_Install, center, map, rotation, WipeMode.Vanish, false, false);
			if (faction != null && sendBPSpawnedSignal)
			{
				QuestUtility.SendQuestTargetSignals(faction.questTags, "PlacedBlueprint", blueprint_Install.Named("SUBJECT"));
			}
			return blueprint_Install;
		}

		// Token: 0x0601348E RID: 78990 RVA: 0x005B8CA8 File Offset: 0x005B6EA8
		public static Blueprint_Install PlaceBlueprintForReinstall(Building buildingToReinstall, IntVec3 center, Map map, Rot4 rotation, Faction faction, bool sendBPSpawnedSignal = true)
		{
			Blueprint_Install blueprint_Install = (Blueprint_Install)ThingMaker.MakeThing(buildingToReinstall.def.installBlueprintDef, null);
			blueprint_Install.SetBuildingToReinstall(buildingToReinstall);
			blueprint_Install.SetFactionDirect(faction);
			GenSpawn.Spawn(blueprint_Install, center, map, rotation, WipeMode.Vanish, false, false);
			if (faction != null && sendBPSpawnedSignal)
			{
				QuestUtility.SendQuestTargetSignals(faction.questTags, "PlacedBlueprint", blueprint_Install.Named("SUBJECT"));
			}
			return blueprint_Install;
		}

		// Token: 0x0601348F RID: 78991 RVA: 0x005B8D10 File Offset: 0x005B6F10
		public static bool CanBuildOnTerrain(BuildableDef entDef, IntVec3 c, Map map, Rot4 rot, Thing thingToIgnore = null, ThingDef stuffDef = null)
		{
			if (entDef is TerrainDef && !c.GetTerrain(map).changeable)
			{
				return false;
			}
			TerrainAffordanceDef terrainAffordanceNeed = entDef.GetTerrainAffordanceNeed(stuffDef);
			if (terrainAffordanceNeed != null)
			{
				CellRect cellRect = GenAdj.OccupiedRect(c, rot, entDef.Size);
				cellRect.ClipInsideMap(map);
				foreach (IntVec3 c2 in cellRect)
				{
					TerrainDef terrainDef = map.terrainGrid.TerrainAt(c2);
					if (!c2.GetAffordances(map).Contains(terrainAffordanceNeed))
					{
						return false;
					}
					ThingDef thingDef = entDef as ThingDef;
					if (thingDef != null && thingDef.building.crater && terrainDef.preventCraters)
					{
						return false;
					}
					List<Thing> thingList = c2.GetThingList(map);
					for (int i = 0; i < thingList.Count; i++)
					{
						if (thingList[i] != thingToIgnore)
						{
							TerrainDef terrainDef2 = thingList[i].def.entityDefToBuild as TerrainDef;
							if (terrainDef2 != null && !terrainDef2.affordances.Contains(terrainAffordanceNeed))
							{
								return false;
							}
						}
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x06013490 RID: 78992 RVA: 0x005B8E4C File Offset: 0x005B704C
		public static Thing MiniToInstallOrBuildingToReinstall(Blueprint b)
		{
			Blueprint_Install blueprint_Install = b as Blueprint_Install;
			if (blueprint_Install != null)
			{
				return blueprint_Install.MiniToInstallOrBuildingToReinstall;
			}
			return null;
		}

		// Token: 0x06013491 RID: 78993 RVA: 0x005B8E6C File Offset: 0x005B706C
		public static bool CanConstruct(Thing t, Pawn pawn, WorkTypeDef workType, bool forced = false, JobDef jobForReservation = null)
		{
			if (!forced && !pawn.workSettings.WorkIsActive(workType))
			{
				JobFailReason.Is("NotAssignedToWorkType".Translate(workType.gerundLabel).CapitalizeFirst(), null);
				return false;
			}
			return GenConstruct.CanConstruct(t, pawn, workType == WorkTypeDefOf.Construction, forced, jobForReservation);
		}

		// Token: 0x06013492 RID: 78994 RVA: 0x005B8EC8 File Offset: 0x005B70C8
		public static bool CanConstruct(Thing t, Pawn p, bool checkSkills = true, bool forced = false, JobDef jobForReservation = null)
		{
			GenConstruct.tmpIdeoMemberNames.Clear();
			if (GenConstruct.FirstBlockingThing(t, p) != null)
			{
				return false;
			}
			if (!GenConstruct.CanTouchTargetFromValidCell(t, p))
			{
				return false;
			}
			if (jobForReservation != null)
			{
				if (!p.Spawned)
				{
					return false;
				}
				if (!p.Map.reservationManager.OnlyReservationsForJobDef(t, jobForReservation, false))
				{
					Pawn pawn = p.Map.reservationManager.FirstRespectedReserver(t, p, null);
					if (pawn != null)
					{
						JobFailReason.Is("ReservedBy".Translate(pawn.LabelShort, pawn), null);
					}
					return false;
				}
				if (!p.CanReach(t, PathEndMode.Touch, forced ? Danger.Deadly : p.NormalMaxDanger(), false, false, TraverseMode.ByPawn))
				{
					JobFailReason.Is("NoPath".Translate(), null);
					return false;
				}
			}
			else if (!p.CanReserveAndReach(t, PathEndMode.Touch, forced ? Danger.Deadly : p.NormalMaxDanger(), 1, -1, null, forced))
			{
				return false;
			}
			if (t.IsBurning())
			{
				return false;
			}
			if (checkSkills)
			{
				if (p.skills != null)
				{
					if (p.skills.GetSkill(SkillDefOf.Construction).Level < t.def.constructionSkillPrerequisite)
					{
						JobFailReason.Is(GenConstruct.SkillTooLowTrans.Formatted(SkillDefOf.Construction.LabelCap), null);
						return false;
					}
					if (p.skills.GetSkill(SkillDefOf.Artistic).Level < t.def.artisticSkillPrerequisite)
					{
						JobFailReason.Is(GenConstruct.SkillTooLowTrans.Formatted(SkillDefOf.Artistic.LabelCap), null);
						return false;
					}
				}
				if (p.IsColonyMech)
				{
					if (p.RaceProps.mechFixedSkillLevel < t.def.constructionSkillPrerequisite)
					{
						JobFailReason.Is(GenConstruct.SkillTooLowTrans.Formatted(SkillDefOf.Construction.LabelCap), null);
						return false;
					}
					if (p.RaceProps.mechFixedSkillLevel < t.def.artisticSkillPrerequisite)
					{
						JobFailReason.Is(GenConstruct.SkillTooLowTrans.Formatted(SkillDefOf.Artistic.LabelCap), null);
						return false;
					}
				}
			}
			bool flag = t is Blueprint_Install;
			if (p.Ideo != null && !p.Ideo.MembersCanBuild(t) && !flag)
			{
				foreach (Ideo ideo in Find.IdeoManager.IdeosListForReading)
				{
					if (ideo.MembersCanBuild(t))
					{
						GenConstruct.tmpIdeoMemberNames.Add(ideo.memberName);
					}
				}
				if (GenConstruct.tmpIdeoMemberNames.Any<string>())
				{
					JobFailReason.Is("OnlyMembersCanBuild".Translate(GenConstruct.tmpIdeoMemberNames.ToCommaList(true, false)), null);
				}
				return false;
			}
			if (t.def.IsBlueprint || t.def.IsFrame)
			{
				ThingDef thingDef = t.def.entityDefToBuild as ThingDef;
				if (thingDef != null)
				{
					if (thingDef.building != null && thingDef.building.isAttachment)
					{
						Thing wallAttachedTo = GenConstruct.GetWallAttachedTo(t);
						if (wallAttachedTo == null || wallAttachedTo.def.IsBlueprint || wallAttachedTo.def.IsFrame)
						{
							return false;
						}
					}
					NamedArgument arg = p.Named(HistoryEventArgsNames.Doer);
					if (!new HistoryEvent(HistoryEventDefOf.BuildSpecificDef, arg, thingDef.Named(HistoryEventArgsNames.Building)).Notify_PawnAboutToDo_Job())
					{
						return false;
					}
					if (thingDef.building != null && thingDef.building.IsTurret && !thingDef.HasComp(typeof(CompMannable)) && !new HistoryEvent(HistoryEventDefOf.BuiltAutomatedTurret, arg).Notify_PawnAboutToDo_Job())
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06013493 RID: 78995 RVA: 0x005B9288 File Offset: 0x005B7488
		public static AcceptanceReport CanPlaceBlueprintAt(BuildableDef entDef, IntVec3 center, Rot4 rot, Map map, bool godMode = false, Thing thingToIgnore = null, Thing thing = null, ThingDef stuffDef = null, bool ignoreEdgeArea = false, bool ignoreInteractionSpots = false, bool ignoreClearableFreeBuildings = false)
		{
			CellRect cellRect = GenAdj.OccupiedRect(center, rot, entDef.Size);
			if (stuffDef == null && thing != null)
			{
				stuffDef = thing.Stuff;
			}
			foreach (IntVec3 c in cellRect)
			{
				if (!c.InBounds(map))
				{
					return new AcceptanceReport("OutOfBounds".Translate());
				}
				if (c.InNoBuildEdgeArea(map) && !godMode && !ignoreEdgeArea)
				{
					return "TooCloseToMapEdge".Translate();
				}
			}
			if (center.Fogged(map))
			{
				return "CannotPlaceInUndiscovered".Translate();
			}
			List<Thing> thingList = center.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing2 = thingList[i];
				if (thing2 != thingToIgnore && thing2.Position == center && thing2.Rotation == rot)
				{
					BuildableDef def = thing2.def;
					if (def == null || !GenConstruct.CanReplace(def, entDef, thing2.Stuff, stuffDef))
					{
						if (thing2.def == entDef)
						{
							return new AcceptanceReport("IdenticalThingExists".Translate());
						}
						if (thing2.def.entityDefToBuild == entDef)
						{
							if (thing2 is Blueprint)
							{
								return new AcceptanceReport("IdenticalBlueprintExists".Translate());
							}
							return new AcceptanceReport("IdenticalThingExists".Translate());
						}
					}
				}
			}
			ThingDef thingDef = entDef as ThingDef;
			if (thingDef != null && thingDef.HasSingleOrMultipleInteractionCells)
			{
				AcceptanceReport result = GenConstruct.InteractionCellStandable(thingDef, center, rot, map, thingToIgnore);
				if (!result.Accepted)
				{
					return result;
				}
			}
			if (!ignoreInteractionSpots)
			{
				AcceptanceReport result2 = GenConstruct.NotBlockingAnyInteractionCells(entDef, center, rot, map, thingToIgnore);
				if (!result2.Accepted)
				{
					return result2;
				}
			}
			TerrainDef terrainDef = entDef as TerrainDef;
			if (terrainDef != null)
			{
				if (terrainDef.isFoundation && map.terrainGrid.FoundationAt(center) != null)
				{
					return new AcceptanceReport("FoundationAlreadyExists".Translate());
				}
				if (map.terrainGrid.TerrainAt(center) == terrainDef)
				{
					return new AcceptanceReport("TerrainIsAlready".Translate(terrainDef.label));
				}
				TerrainDef terrainDef2 = map.terrainGrid.TempTerrainAt(center);
				if (terrainDef2 != null && terrainDef2.Removable && (!terrainDef.bridge || !terrainDef2.tempTerrain.replaceableByBridge))
				{
					return new AcceptanceReport("TerrainUnremovable".Translate());
				}
				if (map.designationManager.DesignationAt(center, DesignationDefOf.SmoothFloor) != null)
				{
					return new AcceptanceReport("SpaceBeingSmoothed".Translate());
				}
			}
			if (GenConstruct.CanBuildOnTerrain(entDef, center, map, rot, thingToIgnore, stuffDef))
			{
				if (ModsConfig.RoyaltyActive)
				{
					List<Thing> list = map.listerThings.ThingsOfDef(ThingDefOf.MonumentMarker);
					for (int j = 0; j < list.Count; j++)
					{
						MonumentMarker monumentMarker = (MonumentMarker)list[j];
						if (!monumentMarker.complete && !monumentMarker.AllowsPlacingBlueprint(entDef, center, rot, stuffDef))
						{
							return new AcceptanceReport("BlueprintWouldCollideWithMonument".Translate());
						}
					}
				}
				if (!godMode)
				{
					foreach (IntVec3 c2 in cellRect)
					{
						thingList = c2.GetThingList(map);
						for (int k = 0; k < thingList.Count; k++)
						{
							Thing thing3 = thingList[k];
							if (thing3 != thingToIgnore)
							{
								Building building = thing3 as Building;
								if ((building == null || !building.IsClearableFreeBuilding || !ignoreClearableFreeBuildings) && !GenConstruct.CanPlaceBlueprintOver(entDef, thing3.def, stuffDef, thing3.Stuff))
								{
									return new AcceptanceReport("SpaceAlreadyOccupied".Translate());
								}
							}
						}
					}
				}
				if (entDef.PlaceWorkers != null)
				{
					for (int l = 0; l < entDef.PlaceWorkers.Count; l++)
					{
						AcceptanceReport result3 = entDef.PlaceWorkers[l].AllowsPlacing(entDef, center, rot, map, thingToIgnore, thing);
						if (!result3.Accepted)
						{
							return result3;
						}
					}
				}
				return AcceptanceReport.WasAccepted;
			}
			if (entDef.GetTerrainAffordanceNeed(stuffDef) == null)
			{
				return new AcceptanceReport("TerrainCannotSupport".Translate(entDef).CapitalizeFirst());
			}
			if (entDef.useStuffTerrainAffordance && stuffDef != null)
			{
				return new AcceptanceReport("TerrainCannotSupport_TerrainAffordanceFromStuff".Translate(entDef, entDef.GetTerrainAffordanceNeed(stuffDef), stuffDef).CapitalizeFirst());
			}
			return new AcceptanceReport("TerrainCannotSupport_TerrainAffordance".Translate(entDef, entDef.GetTerrainAffordanceNeed(stuffDef)).CapitalizeFirst());
		}

		// Token: 0x06013494 RID: 78996 RVA: 0x005B977C File Offset: 0x005B797C
		public static bool CanReplace(BuildableDef placing, BuildableDef existing, ThingDef placingStuff = null, ThingDef existingStuff = null)
		{
			ThingDef thingDef = placing as ThingDef;
			ThingDef thingDef2 = existing as ThingDef;
			if (thingDef == null || thingDef2 == null)
			{
				return false;
			}
			BuildableDef buildableDef = thingDef.entityDefToBuild ?? thingDef;
			BuildableDef buildableDef2 = thingDef2.entityDefToBuild ?? thingDef2;
			return (buildableDef != buildableDef2 || (buildableDef.MadeFromStuff && placingStuff != existingStuff)) && GenConstruct.HasMatchingReplacementTag(thingDef, thingDef2);
		}

		// Token: 0x06013495 RID: 78997 RVA: 0x005B97D0 File Offset: 0x005B79D0
		public static bool HasMatchingReplacementTag(ThingDef a, ThingDef b)
		{
			if (a.replaceTags == null || b.replaceTags == null)
			{
				return false;
			}
			foreach (string item in a.replaceTags)
			{
				if (b.replaceTags.Contains(item))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06013496 RID: 78998 RVA: 0x005B9844 File Offset: 0x005B7A44
		public static AcceptanceReport InteractionCellStandable(ThingDef thingDef, IntVec3 center, Rot4 rot, Map map, Thing thingToIgnore = null)
		{
			foreach (IntVec3 c in ThingUtility.InteractionCellsWhenAt(thingDef, center, rot, map, false))
			{
				if (!c.InBounds(map))
				{
					return new AcceptanceReport("InteractionSpotOutOfBounds".Translate());
				}
				List<Thing> list = map.thingGrid.ThingsListAtFast(c);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i] != thingToIgnore)
					{
						if (list[i].def.passability != Traversability.Standable || list[i].def == thingDef)
						{
							return new AcceptanceReport("InteractionSpotBlocked".Translate(list[i].LabelNoCount, list[i]).CapitalizeFirst());
						}
						BuildableDef entityDefToBuild = list[i].def.entityDefToBuild;
						if (entityDefToBuild != null && (entityDefToBuild.passability != Traversability.Standable || entityDefToBuild == thingDef))
						{
							return new AcceptanceReport("InteractionSpotWillBeBlocked".Translate(list[i].LabelNoCount, list[i]).CapitalizeFirst());
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06013497 RID: 78999 RVA: 0x005B99D4 File Offset: 0x005B7BD4
		public static AcceptanceReport NotBlockingAnyInteractionCells(BuildableDef entDef, IntVec3 center, Rot4 rot, Map map, Thing thingToIgnore = null)
		{
			CellRect cellRect = GenAdj.OccupiedRect(center, rot, entDef.Size);
			foreach (IntVec3 c in GenAdj.CellsAdjacentCardinal(center, rot, entDef.Size))
			{
				if (c.InBounds(map))
				{
					List<Thing> thingList = c.GetThingList(map);
					for (int i = 0; i < thingList.Count; i++)
					{
						Thing thing = thingList[i];
						if (thing != thingToIgnore)
						{
							Blueprint blueprint = thing as Blueprint;
							ThingDef thingDef2;
							if (blueprint != null)
							{
								ThingDef thingDef = blueprint.def.entityDefToBuild as ThingDef;
								if (thingDef == null)
								{
									goto IL_169;
								}
								thingDef2 = thingDef;
							}
							else
							{
								Frame frame = thing as Frame;
								if (frame != null)
								{
									ThingDef thingDef3 = frame.def.entityDefToBuild as ThingDef;
									if (thingDef3 == null)
									{
										goto IL_169;
									}
									thingDef2 = thingDef3;
								}
								else
								{
									thingDef2 = thing.def;
								}
							}
							if (thingDef2.HasSingleOrMultipleInteractionCells && (entDef.passability != Traversability.Standable || entDef == thingDef2))
							{
								foreach (IntVec3 c2 in ThingUtility.InteractionCellsWhenAt(thingDef2, thing.Position, thing.Rotation, thing.Map, false))
								{
									if (cellRect.Contains(c2))
									{
										return new AcceptanceReport("WouldBlockInteractionSpot".Translate(entDef.label, thingDef2.label).CapitalizeFirst());
									}
								}
							}
						}
						IL_169:;
					}
				}
			}
			return true;
		}

		// Token: 0x06013498 RID: 79000 RVA: 0x005B9BB4 File Offset: 0x005B7DB4
		public static BuildableDef BuiltDefOf(ThingDef def)
		{
			return def.entityDefToBuild ?? def;
		}

		// Token: 0x06013499 RID: 79001 RVA: 0x005B9BC4 File Offset: 0x005B7DC4
		public static bool CanPlaceBlueprintOver(BuildableDef newDef, ThingDef oldDef, ThingDef newStuff = null, ThingDef oldStuff = null)
		{
			GenConstruct.<>c__DisplayClass20_0 CS$<>8__locals1;
			CS$<>8__locals1.oldDef = oldDef;
			if (CS$<>8__locals1.oldDef.EverHaulable)
			{
				return true;
			}
			TerrainDef terrainDef = newDef as TerrainDef;
			if (terrainDef != null)
			{
				if (CS$<>8__locals1.oldDef.IsBlueprint || CS$<>8__locals1.oldDef.IsFrame)
				{
					if (CS$<>8__locals1.oldDef.entityDefToBuild.terrainAffordanceNeeded != null && !terrainDef.affordances.Contains(CS$<>8__locals1.oldDef.entityDefToBuild.terrainAffordanceNeeded))
					{
						return false;
					}
				}
				else if (CS$<>8__locals1.oldDef.category == ThingCategory.Building && CS$<>8__locals1.oldDef.terrainAffordanceNeeded != null && !terrainDef.affordances.Contains(CS$<>8__locals1.oldDef.terrainAffordanceNeeded))
				{
					return false;
				}
			}
			CS$<>8__locals1.newThingDef = (newDef as ThingDef);
			CS$<>8__locals1.oldDefBuilt = GenConstruct.BuiltDefOf(CS$<>8__locals1.oldDef);
			ThingDef thingDef = CS$<>8__locals1.oldDefBuilt as ThingDef;
			if (newDef.blocksAltitudes != null && newDef.blocksAltitudes.Contains(CS$<>8__locals1.oldDef.altitudeLayer))
			{
				return false;
			}
			BuildableDef oldDefBuilt = CS$<>8__locals1.oldDefBuilt;
			if (((oldDefBuilt != null) ? oldDefBuilt.blocksAltitudes : null) != null && CS$<>8__locals1.oldDefBuilt.blocksAltitudes.Contains(newDef.altitudeLayer))
			{
				return false;
			}
			if (GenConstruct.CanReplace(CS$<>8__locals1.newThingDef, CS$<>8__locals1.oldDef, newStuff, oldStuff))
			{
				return true;
			}
			if (newDef.ForceAllowPlaceOver(CS$<>8__locals1.oldDef))
			{
				return true;
			}
			if (CS$<>8__locals1.oldDef.category == ThingCategory.Plant && CS$<>8__locals1.oldDef.passability == Traversability.Impassable && CS$<>8__locals1.newThingDef != null && CS$<>8__locals1.newThingDef.category == ThingCategory.Building && !CS$<>8__locals1.newThingDef.building.canPlaceOverImpassablePlant)
			{
				return false;
			}
			if (CS$<>8__locals1.oldDef.category == ThingCategory.Building || CS$<>8__locals1.oldDef.IsBlueprint || CS$<>8__locals1.oldDef.IsFrame)
			{
				if (CS$<>8__locals1.newThingDef != null)
				{
					if (!CS$<>8__locals1.newThingDef.IsEdifice())
					{
						return (CS$<>8__locals1.oldDef.building == null || CS$<>8__locals1.oldDef.building.canBuildNonEdificesUnder) && (!CS$<>8__locals1.newThingDef.EverTransmitsPower || !CS$<>8__locals1.oldDef.EverTransmitsPower);
					}
					if (GenConstruct.<CanPlaceBlueprintOver>g__IsEdificeOverNonEdifice|20_0(ref CS$<>8__locals1))
					{
						if (CS$<>8__locals1.newThingDef.building != null && !CS$<>8__locals1.newThingDef.building.canBuildNonEdificesUnder)
						{
							return false;
						}
						ThingDef thingDef2 = CS$<>8__locals1.oldDefBuilt as ThingDef;
						return thingDef2 == null || !thingDef2.building.isAttachment || CS$<>8__locals1.newThingDef.Fillage != FillCategory.Full;
					}
					else
					{
						if (((thingDef != null) ? thingDef.building : null) != null && (thingDef.building.isPlaceOverableWall || thingDef.IsSmoothed) && CS$<>8__locals1.newThingDef.building != null && CS$<>8__locals1.newThingDef.building.canPlaceOverWall)
						{
							return true;
						}
						BuildingProperties building = CS$<>8__locals1.newThingDef.building;
						if ((building == null || !building.isPowerConduit) && thingDef != null)
						{
							BuildingProperties building2 = thingDef.building;
							bool? flag = (building2 != null) ? new bool?(building2.isPowerConduit) : null;
							bool flag2 = true;
							if (flag.GetValueOrDefault() == flag2 & flag != null)
							{
								return true;
							}
						}
					}
				}
				if (newDef is TerrainDef)
				{
					ThingDef thingDef3 = CS$<>8__locals1.oldDefBuilt as ThingDef;
					if (thingDef3 != null && thingDef3.CoexistsWithFloors)
					{
						return true;
					}
				}
				return CS$<>8__locals1.oldDefBuilt is TerrainDef && !(newDef is TerrainDef);
			}
			return true;
		}

		// Token: 0x0601349A RID: 79002 RVA: 0x005B9F14 File Offset: 0x005B8114
		public static Thing FirstBlockingThing(Thing constructible, Pawn pawnToIgnore)
		{
			Blueprint blueprint = constructible as Blueprint;
			Thing thing;
			if (blueprint != null)
			{
				thing = GenConstruct.MiniToInstallOrBuildingToReinstall(blueprint);
			}
			else
			{
				thing = null;
			}
			foreach (IntVec3 c in constructible.OccupiedRect())
			{
				List<Thing> thingList = c.GetThingList(constructible.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Thing thing2 = thingList[i];
					if (GenConstruct.BlocksConstruction(constructible, thing2) && thing2 != pawnToIgnore && thing2 != thing)
					{
						return thing2;
					}
				}
			}
			return null;
		}

		// Token: 0x0601349B RID: 79003 RVA: 0x005B9FC4 File Offset: 0x005B81C4
		public static bool CanTouchTargetFromValidCell(Thing constructible, Pawn worker)
		{
			IntVec3 intVec;
			return RCellFinder.TryFindGoodAdjacentSpotToTouch(worker, constructible, out intVec);
		}

		// Token: 0x0601349C RID: 79004 RVA: 0x005B9FDC File Offset: 0x005B81DC
		public static Job HandleBlockingThingJob(Thing constructible, Pawn worker, bool forced = false)
		{
			Thing thing = GenConstruct.FirstBlockingThing(constructible, worker);
			if (thing == null)
			{
				return null;
			}
			if (thing.def.category == ThingCategory.Plant)
			{
				if (!PlantUtility.PawnWillingToCutPlant_Job(thing, worker))
				{
					return null;
				}
				if (PlantUtility.TreeMarkedForExtraction(thing))
				{
					JobFailReason.Is(GenConstruct.TreeMarkedForExtraction, null);
					return null;
				}
				if (worker.CanReserveAndReach(thing, PathEndMode.ClosestTouch, worker.NormalMaxDanger(), 1, -1, null, forced))
				{
					return JobMaker.MakeJob(JobDefOf.CutPlant, thing);
				}
			}
			else if (thing.def.category == ThingCategory.Item)
			{
				if (thing.def.EverHaulable)
				{
					return HaulAIUtility.HaulAsideJobFor(worker, thing);
				}
				string[] array = new string[6];
				array[0] = "Never haulable ";
				int num = 1;
				Thing thing2 = thing;
				array[num] = ((thing2 != null) ? thing2.ToString() : null);
				array[2] = " blocking ";
				array[3] = constructible.ToStringSafe<Thing>();
				array[4] = " at ";
				array[5] = constructible.Position.ToString();
				Log.ErrorOnce(string.Concat(array), 6429262);
			}
			else if (thing.def.category == ThingCategory.Building)
			{
				if (((Building)thing).DeconstructibleBy(worker.Faction))
				{
					if (worker.WorkTypeIsDisabled(WorkTypeDefOf.Construction) || (worker.workSettings != null && !worker.workSettings.WorkIsActive(WorkTypeDefOf.Construction)))
					{
						JobFailReason.Is(GenConstruct.IncapableOfDeconstruction, null);
						return null;
					}
					if (!forced && thing.IsForbidden(worker))
					{
						return null;
					}
					if (worker.CanReserveAndReach(thing, PathEndMode.Touch, worker.NormalMaxDanger(), 1, -1, null, forced))
					{
						Job job = JobMaker.MakeJob(JobDefOf.Deconstruct, thing);
						job.ignoreDesignations = true;
						return job;
					}
				}
				if (thing.def.mineable)
				{
					if (worker.WorkTypeIsDisabled(WorkTypeDefOf.Mining) || (worker.workSettings != null && !worker.workSettings.WorkIsActive(WorkTypeDefOf.Mining)))
					{
						JobFailReason.Is(GenConstruct.IncapableOfMining, null);
						return null;
					}
					if (worker.CanReserveAndReach(thing, PathEndMode.Touch, worker.NormalMaxDanger(), 1, -1, null, forced))
					{
						Job job2 = JobMaker.MakeJob(JobDefOf.Mine, thing);
						job2.ignoreDesignations = true;
						return job2;
					}
				}
			}
			return null;
		}

		// Token: 0x0601349D RID: 79005 RVA: 0x005BA1E8 File Offset: 0x005B83E8
		public static bool BlocksConstruction(Thing constructible, Thing t)
		{
			if (t == constructible)
			{
				return false;
			}
			ThingDef thingDef = GenConstruct.BlueprintDefOf(constructible);
			if (t.def.category == ThingCategory.Building && GenSpawn.SpawningWipes(thingDef.entityDefToBuild, t.def, false))
			{
				return true;
			}
			if (t.def.category == ThingCategory.Plant)
			{
				return t.def.plant.harvestWork > ThingDefOf.Plant_Dandelion.plant.harvestWork && (!(thingDef.entityDefToBuild is TerrainDef) || !t.Spawned || !(t.Position.GetEdifice(t.Map) is IPlantToGrowSettable));
			}
			if (!thingDef.clearBuildingArea)
			{
				return false;
			}
			if (thingDef.entityDefToBuild.ForceAllowPlaceOver(t.def))
			{
				return false;
			}
			ThingDef thingDef2 = thingDef.entityDefToBuild as ThingDef;
			if (thingDef2 != null)
			{
				if (thingDef2.EverTransmitsPower)
				{
					BuildingProperties building = t.def.building;
					if (building != null && building.isPowerConduit)
					{
						BuildingProperties building2 = thingDef2.building;
						if (building2 != null && !building2.isPowerConduit)
						{
							return false;
						}
					}
				}
				if (t.def == ThingDefOf.Wall && thingDef2.building != null && thingDef2.building.canPlaceOverWall)
				{
					return false;
				}
				if (t.def.Fillage != FillCategory.Full && thingDef2.building != null && thingDef2.building.isAttachment)
				{
					return false;
				}
				if (t.def.category == ThingCategory.Item && thingDef2.passability != Traversability.Standable && thingDef2.surfaceType == SurfaceType.None)
				{
					return true;
				}
			}
			if (t.def.IsEdifice() && thingDef2.IsEdifice())
			{
				return true;
			}
			bool flag = thingDef.entityDefToBuild.passability == Traversability.Impassable || thingDef.forceMoveItemsBeforeConstruction;
			if (t.def.category == ThingCategory.Item && flag)
			{
				return true;
			}
			Pawn pawn = t as Pawn;
			if (pawn != null && !pawn.IsHiddenFromPlayer())
			{
				return true;
			}
			if (t.def.Fillage < FillCategory.Partial || (!t.def.IsEdifice() && (t.def.entityDefToBuild == null || !t.def.entityDefToBuild.IsEdifice())))
			{
				return false;
			}
			if (thingDef2.blocksAltitudes == null)
			{
				return true;
			}
			List<AltitudeLayer> list;
			if ((list = t.def.blocksAltitudes) == null)
			{
				BuildableDef entityDefToBuild = t.def.entityDefToBuild;
				list = ((entityDefToBuild != null) ? entityDefToBuild.blocksAltitudes : null);
			}
			List<AltitudeLayer> list2 = list;
			return list2 != null && list2.SharesElementWith(thingDef2.blocksAltitudes);
		}

		// Token: 0x0601349E RID: 79006 RVA: 0x005BA438 File Offset: 0x005B8638
		private static ThingDef BlueprintDefOf(Thing constructible)
		{
			ThingDef result;
			if (constructible is Blueprint)
			{
				result = constructible.def;
			}
			else if (constructible is Frame)
			{
				result = constructible.def.entityDefToBuild.blueprintDef;
			}
			else
			{
				result = constructible.def.blueprintDef;
			}
			return result;
		}

		// Token: 0x0601349F RID: 79007 RVA: 0x005BA480 File Offset: 0x005B8680
		public static bool TerrainCanSupport(CellRect rect, Map map, ThingDef thing)
		{
			using (CellRect.Enumerator enumerator = rect.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.SupportsStructureType(map, thing.terrainAffordanceNeeded))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060134A0 RID: 79008 RVA: 0x005BA4DC File Offset: 0x005B86DC
		public static List<Thing> GetAttachedBuildings(Thing thing)
		{
			List<Thing> list = new List<Thing>();
			if (thing == null)
			{
				return list;
			}
			ThingDef thingDef = GenConstruct.BuiltDefOf(thing.def) as ThingDef;
			if (((thingDef != null) ? thingDef.building : null) == null || !thingDef.building.supportsWallAttachments)
			{
				return list;
			}
			for (int i = 0; i < 4; i++)
			{
				IntVec3 c = thing.Position + GenAdj.CardinalDirections[i];
				if (c.InBounds(thing.Map))
				{
					foreach (Thing thing2 in c.GetThingList(thing.Map))
					{
						ThingDef thingDef2 = GenConstruct.BuiltDefOf(thing2.def) as ThingDef;
						if (((thingDef2 != null) ? thingDef2.building : null) != null && thingDef2.building.isAttachment && GenMath.PositiveMod(thing2.Rotation.AsInt - 2, 4) == i)
						{
							list.Add(thing2);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x060134A1 RID: 79009 RVA: 0x005BA5F8 File Offset: 0x005B87F8
		public static Thing GetWallAttachedTo(Thing thing)
		{
			if (thing == null)
			{
				return null;
			}
			ThingDef thingDef = GenConstruct.BuiltDefOf(thing.def) as ThingDef;
			if (((thingDef != null) ? thingDef.building : null) == null || !thingDef.building.isAttachment)
			{
				return null;
			}
			return GenConstruct.GetWallAttachedTo(thing.Position, thing.Rotation, thing.Map);
		}

		// Token: 0x060134A2 RID: 79010 RVA: 0x005BA650 File Offset: 0x005B8850
		public static Thing GetWallAttachedTo(IntVec3 pos, Rot4 rot, Map map)
		{
			IntVec3 c = pos + GenAdj.CardinalDirections[rot.AsInt];
			if (!c.InBounds(map))
			{
				return null;
			}
			foreach (Thing thing in c.GetThingList(map))
			{
				ThingDef thingDef = GenConstruct.BuiltDefOf(thing.def) as ThingDef;
				if (thingDef != null && thingDef.building != null && thingDef.building.supportsWallAttachments)
				{
					return thing;
				}
			}
			return null;
		}

		// Token: 0x060134A4 RID: 79012 RVA: 0x005BA700 File Offset: 0x005B8900
		[CompilerGenerated]
		internal static bool <CanPlaceBlueprintOver>g__IsEdificeOverNonEdifice|20_0(ref GenConstruct.<>c__DisplayClass20_0 A_0)
		{
			if (!A_0.newThingDef.IsEdifice())
			{
				return false;
			}
			if (A_0.oldDef != null && A_0.oldDef.category == ThingCategory.Building && !A_0.oldDef.IsEdifice())
			{
				return true;
			}
			ThingDef thingDef = A_0.oldDefBuilt as ThingDef;
			return thingDef != null && thingDef.category == ThingCategory.Building && !thingDef.IsEdifice();
		}

		// Token: 0x0400D605 RID: 54789
		public const float ConstructionSpeedGlobalFactor = 1.7f;

		// Token: 0x0400D606 RID: 54790
		private static string SkillTooLowTrans;

		// Token: 0x0400D607 RID: 54791
		private static string IncapableOfDeconstruction;

		// Token: 0x0400D608 RID: 54792
		private static string IncapableOfMining;

		// Token: 0x0400D609 RID: 54793
		private static string TreeMarkedForExtraction;

		// Token: 0x0400D60A RID: 54794
		private static readonly List<string> tmpIdeoMemberNames = new List<string>();
	}
}
