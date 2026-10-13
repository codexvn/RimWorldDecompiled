using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x020008E4 RID: 2276
	public static class ThingUtility
	{
		// Token: 0x06003A53 RID: 14931 RVA: 0x00135B4B File Offset: 0x00133D4B
		public static bool DestroyedOrNull(this Thing t)
		{
			return t == null || t.Destroyed;
		}

		// Token: 0x06003A54 RID: 14932 RVA: 0x00135B58 File Offset: 0x00133D58
		public static void DestroyOrPassToWorld(this Thing t, DestroyMode mode = DestroyMode.Vanish)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				if (!Find.WorldPawns.Contains(pawn))
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
					return;
				}
			}
			else
			{
				t.Destroy(mode);
			}
		}

		// Token: 0x06003A55 RID: 14933 RVA: 0x00135B90 File Offset: 0x00133D90
		public static int TryAbsorbStackNumToTake(Thing thing, Thing other, bool respectStackLimit)
		{
			int result;
			if (respectStackLimit)
			{
				result = Mathf.Min(other.stackCount, thing.def.stackLimit - thing.stackCount);
			}
			else
			{
				result = other.stackCount;
			}
			return result;
		}

		// Token: 0x06003A56 RID: 14934 RVA: 0x00135BC8 File Offset: 0x00133DC8
		public static int RoundedResourceStackCount(int stackCount)
		{
			if (stackCount > 200)
			{
				return GenMath.RoundTo(stackCount, 10);
			}
			if (stackCount > 100)
			{
				return GenMath.RoundTo(stackCount, 5);
			}
			return stackCount;
		}

		// Token: 0x06003A57 RID: 14935 RVA: 0x00135BE9 File Offset: 0x00133DE9
		public static IntVec3 InteractionCell(IntVec3 interactionOffset, IntVec3 thingCenter, Rot4 rot)
		{
			return interactionOffset.RotatedBy(rot) + thingCenter;
		}

		// Token: 0x06003A58 RID: 14936 RVA: 0x00135BF8 File Offset: 0x00133DF8
		public static List<IntVec3> InteractionCellsWhenAt(ThingDef def, IntVec3 center, Rot4 rot, Map map, bool allowFallbackCell = false)
		{
			ThingUtility.InteractionCellsWhenAt(ThingUtility.tmpInteractionCells, def, center, rot, map, allowFallbackCell);
			return ThingUtility.tmpInteractionCells;
		}

		// Token: 0x06003A59 RID: 14937 RVA: 0x00135C10 File Offset: 0x00133E10
		public static void InteractionCellsWhenAt(List<IntVec3> listToPopulate, ThingDef def, IntVec3 center, Rot4 rot, Map map, bool allowFallbackCell = false)
		{
			listToPopulate.Clear();
			if (!def.multipleInteractionCellOffsets.NullOrEmpty<IntVec3>())
			{
				using (List<IntVec3>.Enumerator enumerator = def.multipleInteractionCellOffsets.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IntVec3 interactionOffset = enumerator.Current;
						listToPopulate.Add(ThingUtility.InteractionCell(interactionOffset, center, rot));
					}
					return;
				}
			}
			if (def.hasInteractionCell || allowFallbackCell)
			{
				listToPopulate.Add(ThingUtility.InteractionCellWhenAt(def, center, rot, map));
			}
		}

		// Token: 0x06003A5A RID: 14938 RVA: 0x00135C98 File Offset: 0x00133E98
		public static IntVec3 InteractionCellWhenAt(ThingDef def, IntVec3 center, Rot4 rot, Map map)
		{
			if (def.hasInteractionCell)
			{
				return ThingUtility.InteractionCell(def.interactionCellOffset, center, rot);
			}
			if (def.Size.x == 1 && def.Size.z == 1)
			{
				for (int i = 0; i < 8; i++)
				{
					IntVec3 intVec = center + GenAdj.AdjacentCells[i];
					if (intVec.Standable(map) && intVec.GetDoor(map) == null && ReachabilityImmediate.CanReachImmediate(intVec, center, map, PathEndMode.Touch, null))
					{
						return intVec;
					}
				}
				for (int j = 0; j < 8; j++)
				{
					IntVec3 intVec2 = center + GenAdj.AdjacentCells[j];
					if (intVec2.Standable(map) && ReachabilityImmediate.CanReachImmediate(intVec2, center, map, PathEndMode.Touch, null))
					{
						return intVec2;
					}
				}
				for (int k = 0; k < 8; k++)
				{
					IntVec3 intVec3 = center + GenAdj.AdjacentCells[k];
					if (intVec3.Walkable(map) && ReachabilityImmediate.CanReachImmediate(intVec3, center, map, PathEndMode.Touch, null))
					{
						return intVec3;
					}
				}
				return center;
			}
			List<IntVec3> list = GenAdjFast.AdjacentCells8Way(center, rot, def.size);
			CellRect rect = GenAdj.OccupiedRect(center, rot, def.size);
			for (int l = 0; l < list.Count; l++)
			{
				if (list[l].Standable(map) && list[l].GetDoor(map) == null && ReachabilityImmediate.CanReachImmediate(list[l], rect, map, PathEndMode.Touch, null))
				{
					return list[l];
				}
			}
			for (int m = 0; m < list.Count; m++)
			{
				if (list[m].Standable(map) && ReachabilityImmediate.CanReachImmediate(list[m], rect, map, PathEndMode.Touch, null))
				{
					return list[m];
				}
			}
			for (int n = 0; n < list.Count; n++)
			{
				if (list[n].Walkable(map) && ReachabilityImmediate.CanReachImmediate(list[n], rect, map, PathEndMode.Touch, null))
				{
					return list[n];
				}
			}
			return center;
		}

		// Token: 0x06003A5B RID: 14939 RVA: 0x00135EA1 File Offset: 0x001340A1
		public static DamageDef PrimaryMeleeWeaponDamageType(ThingDef thing)
		{
			return ThingUtility.PrimaryMeleeWeaponDamageType(thing.tools);
		}

		// Token: 0x06003A5C RID: 14940 RVA: 0x00135EB0 File Offset: 0x001340B0
		public static DamageDef PrimaryMeleeWeaponDamageType(List<Tool> tools)
		{
			if (tools.NullOrEmpty<Tool>())
			{
				return null;
			}
			ManeuverDef maneuverDef = tools.MaxBy((Tool tool) => tool.power).Maneuvers.FirstOrDefault<ManeuverDef>();
			if (maneuverDef == null)
			{
				return null;
			}
			return maneuverDef.verb.meleeDamageDef;
		}

		// Token: 0x06003A5D RID: 14941 RVA: 0x00135F08 File Offset: 0x00134108
		public static Blueprint_Build CheckAutoRebuildOnDestroyed(Thing thing, DestroyMode mode, Map map, BuildableDef buildingDef)
		{
			if (Find.PlaySettings.autoRebuild && mode == DestroyMode.KillFinalize && thing.Faction == Faction.OfPlayer && buildingDef.blueprintDef != null && buildingDef.IsResearchFinished && map.areaManager.Home[thing.Position] && GenConstruct.CanPlaceBlueprintAt(buildingDef, thing.Position, thing.Rotation, map, false, null, null, thing.Stuff, false, false, false).Accepted)
			{
				return GenConstruct.PlaceBlueprintForBuild(buildingDef, thing.Position, map, thing.Rotation, Faction.OfPlayer, thing.Stuff, thing.StyleSourcePrecept, thing.StyleDef, true);
			}
			return null;
		}

		// Token: 0x06003A5E RID: 14942 RVA: 0x00135FB8 File Offset: 0x001341B8
		public static void CheckAutoRebuildTerrainOnDestroyed(TerrainDef terrainDef, IntVec3 pos, Map map)
		{
			if (Find.PlaySettings.autoRebuild && terrainDef.autoRebuildable && terrainDef.blueprintDef != null && terrainDef.IsResearchFinished && map.areaManager.Home[pos] && GenConstruct.CanPlaceBlueprintAt(terrainDef, pos, Rot4.South, map, false, null, null, null, false, false, false).Accepted)
			{
				GenConstruct.PlaceBlueprintForBuild(terrainDef, pos, map, Rot4.South, Faction.OfPlayer, null, null, null, true);
			}
		}

		// Token: 0x06003A5F RID: 14943 RVA: 0x00136034 File Offset: 0x00134234
		public static Pawn FindPawn(List<Thing> things)
		{
			for (int i = 0; i < things.Count; i++)
			{
				Pawn pawn = things[i] as Pawn;
				if (pawn != null)
				{
					return pawn;
				}
				Corpse corpse = things[i] as Corpse;
				if (corpse != null)
				{
					return corpse.InnerPawn;
				}
			}
			return null;
		}

		// Token: 0x06003A60 RID: 14944 RVA: 0x0013607C File Offset: 0x0013427C
		public static TerrainAffordanceDef GetTerrainAffordanceNeed(this BuildableDef def, ThingDef stuffDef = null)
		{
			TerrainAffordanceDef terrainAffordanceNeeded = def.terrainAffordanceNeeded;
			if (stuffDef != null && def.useStuffTerrainAffordance && stuffDef.terrainAffordanceNeeded != null)
			{
				terrainAffordanceNeeded = stuffDef.terrainAffordanceNeeded;
			}
			return terrainAffordanceNeeded;
		}

		// Token: 0x06003A61 RID: 14945 RVA: 0x001360AB File Offset: 0x001342AB
		public static bool HasThingCategory(this Thing thing, ThingCategoryDef thingCategory)
		{
			return !thing.def.thingCategories.NullOrEmpty<ThingCategoryDef>() && thing.def.thingCategories.Contains(thingCategory);
		}

		// Token: 0x06003A62 RID: 14946 RVA: 0x001360D4 File Offset: 0x001342D4
		public static int GetStackCountFromThingList(IEnumerable<Thing> things)
		{
			int num = 0;
			foreach (Thing thing in things)
			{
				num += thing.stackCount;
			}
			return num;
		}

		// Token: 0x06003A63 RID: 14947 RVA: 0x00136124 File Offset: 0x00134324
		public static void FindAllOfType<T>(ThingRequest request, List<T> list) where T : Thing
		{
			list.Clear();
			List<Thing> list2 = Find.CurrentMap.listerThings.ThingsMatching(request);
			for (int i = 0; i < list2.Count; i++)
			{
				T t = list2[i] as T;
				if (t != null)
				{
					list.Add(t);
				}
			}
			List<Pawn> freeColonists = Find.CurrentMap.mapPawns.FreeColonists;
			for (int j = 0; j < freeColonists.Count; j++)
			{
				T t2 = freeColonists[j].carryTracker.CarriedThing as T;
				if (t2 != null)
				{
					list.Add(t2);
				}
				freeColonists[j].inventory.innerContainer.GetThingsOfType<T>(list);
			}
		}

		// Token: 0x06003A64 RID: 14948 RVA: 0x001361E8 File Offset: 0x001343E8
		public static void FindAllOfType<T>(List<T> list) where T : Thing
		{
			list.Clear();
			Find.CurrentMap.listerThings.GetThingsOfType<T>(list);
			List<Pawn> freeColonists = Find.CurrentMap.mapPawns.FreeColonists;
			for (int i = 0; i < freeColonists.Count; i++)
			{
				T t = freeColonists[i].carryTracker.CarriedThing as T;
				if (t != null)
				{
					list.Add(t);
				}
				freeColonists[i].inventory.innerContainer.GetThingsOfType<T>(list);
			}
		}

		// Token: 0x04002A8B RID: 10891
		private static List<IntVec3> tmpInteractionCells = new List<IntVec3>();
	}
}
