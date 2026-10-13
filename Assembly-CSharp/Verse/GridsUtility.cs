using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000433 RID: 1075
	public static class GridsUtility
	{
		// Token: 0x06001B4D RID: 6989 RVA: 0x00089ED8 File Offset: 0x000880D8
		public static float GetTemperature(this IntVec3 loc, Map map)
		{
			return GenTemperature.GetTemperatureForCell(loc, map);
		}

		// Token: 0x06001B4E RID: 6990 RVA: 0x00089EE1 File Offset: 0x000880E1
		public static Region GetRegion(this IntVec3 loc, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			return RegionAndRoomQuery.RegionAt(loc, map, allowedRegionTypes);
		}

		// Token: 0x06001B4F RID: 6991 RVA: 0x00089EEB File Offset: 0x000880EB
		public static District GetDistrict(this IntVec3 loc, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			return RegionAndRoomQuery.DistrictAt(loc, map, allowedRegionTypes);
		}

		// Token: 0x06001B50 RID: 6992 RVA: 0x00089EF8 File Offset: 0x000880F8
		public static bool ConnectedCellCountExceeds(this IntVec3 cell, Map map, int count, RegionType allowed = RegionType.Set_Passable)
		{
			Region region = cell.GetRegion(map, allowed);
			if (region == null)
			{
				return false;
			}
			int num = region.CellCount;
			GridsUtility.connected.Add(region);
			GridsUtility.queue.Enqueue(region);
			Region region2;
			while (GridsUtility.queue.TryDequeue(out region2) && num < count)
			{
				num += region2.CellCount;
				using (List<RegionLink>.Enumerator enumerator = region2.links.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						RegionLink regionLink = enumerator.Current;
						Region otherRegion = regionLink.GetOtherRegion(region2);
						if (GridsUtility.connected.Add(otherRegion))
						{
							GridsUtility.queue.Enqueue(otherRegion);
						}
					}
					continue;
				}
				break;
			}
			GridsUtility.connected.Clear();
			GridsUtility.queue.Clear();
			return num >= count;
		}

		// Token: 0x06001B51 RID: 6993 RVA: 0x00089FC8 File Offset: 0x000881C8
		public static Room GetRoom(this IntVec3 loc, Map map)
		{
			return RegionAndRoomQuery.RoomAt(loc, map, RegionType.Set_All);
		}

		// Token: 0x06001B52 RID: 6994 RVA: 0x00089FD3 File Offset: 0x000881D3
		public static Room GetRoomOrAdjacent(this IntVec3 loc, Map map, RegionType allowedRegionTypes = RegionType.Set_Passable)
		{
			return RegionAndRoomQuery.RoomAtOrAdjacent(loc, map, allowedRegionTypes);
		}

		// Token: 0x06001B53 RID: 6995 RVA: 0x00089FDD File Offset: 0x000881DD
		public static List<Thing> GetThingList(this IntVec3 c, Map map)
		{
			return map.thingGrid.ThingsListAt(c);
		}

		// Token: 0x06001B54 RID: 6996 RVA: 0x00089FEB File Offset: 0x000881EB
		public static float GetSnowDepth(this IntVec3 c, Map map)
		{
			return map.snowGrid.GetDepth(c);
		}

		// Token: 0x06001B55 RID: 6997 RVA: 0x00089FF9 File Offset: 0x000881F9
		public static float GetSandDepth(this IntVec3 c, Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return 0f;
			}
			return map.sandGrid.GetDepth(c);
		}

		// Token: 0x06001B56 RID: 6998 RVA: 0x0008A014 File Offset: 0x00088214
		public static bool Fogged(this Thing t)
		{
			return t.MapHeld.fogGrid.IsFogged(t.PositionHeld);
		}

		// Token: 0x06001B57 RID: 6999 RVA: 0x0008A02C File Offset: 0x0008822C
		public static bool Fogged(this IntVec3 c, Map map)
		{
			return map.fogGrid.IsFogged(c);
		}

		// Token: 0x06001B58 RID: 7000 RVA: 0x0008A03A File Offset: 0x0008823A
		public static RoofDef GetRoof(this IntVec3 c, Map map)
		{
			return map.roofGrid.RoofAt(c);
		}

		// Token: 0x06001B59 RID: 7001 RVA: 0x0008A048 File Offset: 0x00088248
		public static bool Roofed(this IntVec3 c, Map map)
		{
			return map.roofGrid.Roofed(c);
		}

		// Token: 0x06001B5A RID: 7002 RVA: 0x0008A058 File Offset: 0x00088258
		public static bool Filled(this IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			return edifice != null && edifice.def.Fillage == FillCategory.Full;
		}

		// Token: 0x06001B5B RID: 7003 RVA: 0x0008A080 File Offset: 0x00088280
		public static TerrainDef GetTerrain(this IntVec3 c, Map map)
		{
			TerrainDef terrainDef = map.terrainGrid.TerrainAt(c);
			if (terrainDef == null)
			{
				return TerrainDefOf.Soil;
			}
			return terrainDef;
		}

		// Token: 0x06001B5C RID: 7004 RVA: 0x0008A0A4 File Offset: 0x000882A4
		public static List<TerrainAffordanceDef> GetAffordances(this IntVec3 c, Map map)
		{
			if (map.terrainGrid.FoundationAt(c) != null)
			{
				return map.terrainGrid.FoundationAt(c).affordances;
			}
			return map.terrainGrid.TerrainAt(c).affordances;
		}

		// Token: 0x06001B5D RID: 7005 RVA: 0x0008A0D8 File Offset: 0x000882D8
		public static WaterBodyType GetWaterBodyType(this IntVec3 c, Map map)
		{
			if (map.terrainGrid.FoundationAt(c) != null)
			{
				return map.terrainGrid.BaseTerrainAt(c).waterBodyType;
			}
			if (map.terrainGrid.TerrainAt(c).temporary)
			{
				return map.terrainGrid.TerrainAtIgnoreTemp(map.cellIndices.CellToIndex(c)).waterBodyType;
			}
			return map.terrainGrid.TerrainAt(c).waterBodyType;
		}

		// Token: 0x06001B5E RID: 7006 RVA: 0x0008A146 File Offset: 0x00088346
		public static Plan GetPlan(this IntVec3 c, Map map)
		{
			return map.planManager.PlanAt(c);
		}

		// Token: 0x06001B5F RID: 7007 RVA: 0x0008A154 File Offset: 0x00088354
		public static Zone GetZone(this IntVec3 c, Map map)
		{
			return map.zoneManager.ZoneAt(c);
		}

		// Token: 0x06001B60 RID: 7008 RVA: 0x0008A164 File Offset: 0x00088364
		public static Plant GetPlant(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.category == ThingCategory.Plant)
				{
					return (Plant)list[i];
				}
			}
			return null;
		}

		// Token: 0x06001B61 RID: 7009 RVA: 0x0008A1B4 File Offset: 0x000883B4
		public static Thing GetRoofHolderOrImpassable(this IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def.holdsRoof || thingList[i].def.passability == Traversability.Impassable)
				{
					return thingList[i];
				}
			}
			return null;
		}

		// Token: 0x06001B62 RID: 7010 RVA: 0x0008A20C File Offset: 0x0008840C
		public static Thing GetFirstThing(this IntVec3 c, Map map, ThingDef def)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def == def)
				{
					return thingList[i];
				}
			}
			return null;
		}

		// Token: 0x06001B63 RID: 7011 RVA: 0x0008A24C File Offset: 0x0008844C
		public static ThingWithComps GetFirstThingWithComp<TComp>(this IntVec3 c, Map map) where TComp : ThingComp
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].TryGetComp<TComp>() != null)
				{
					return (ThingWithComps)thingList[i];
				}
			}
			return null;
		}

		// Token: 0x06001B64 RID: 7012 RVA: 0x0008A294 File Offset: 0x00088494
		public static T GetFirstThing<T>(this IntVec3 c, Map map) where T : Thing
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				T t = thingList[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06001B65 RID: 7013 RVA: 0x0008A2DF File Offset: 0x000884DF
		public static bool TryGetFirstThing<T>(this IntVec3 c, Map map, out T thing) where T : Thing
		{
			thing = c.GetFirstThing(map);
			return thing != null;
		}

		// Token: 0x06001B66 RID: 7014 RVA: 0x0008A2FC File Offset: 0x000884FC
		public static Thing GetFirstHaulable(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.designateHaulable)
				{
					return list[i];
				}
			}
			return null;
		}

		// Token: 0x06001B67 RID: 7015 RVA: 0x0008A344 File Offset: 0x00088544
		public static Thing GetFirstItem(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.category == ThingCategory.Item)
				{
					return list[i];
				}
			}
			return null;
		}

		// Token: 0x06001B68 RID: 7016 RVA: 0x0008A38C File Offset: 0x0008858C
		public static IEnumerable<Thing> GetItems(this IntVec3 c, Map map)
		{
			List<Thing> thingList = map.thingGrid.ThingsListAt(c);
			int num;
			for (int i = 0; i < thingList.Count; i = num + 1)
			{
				if (thingList[i].def.category == ThingCategory.Item)
				{
					yield return thingList[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06001B69 RID: 7017 RVA: 0x0008A3A4 File Offset: 0x000885A4
		public static int GetMaxItemsAllowedInCell(this IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			if (edifice != null)
			{
				return edifice.MaxItemsInCell;
			}
			return 1;
		}

		// Token: 0x06001B6A RID: 7018 RVA: 0x0008A3C4 File Offset: 0x000885C4
		public static int GetItemCount(this IntVec3 c, Map map)
		{
			int num = 0;
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.category == ThingCategory.Item)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06001B6B RID: 7019 RVA: 0x0008A40C File Offset: 0x0008860C
		public static int GetAllItemsStackCount(this IntVec3 c, Map map, ThingDef itemDef)
		{
			int num = 0;
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def == itemDef)
				{
					num += list[i].stackCount;
				}
			}
			return num;
		}

		// Token: 0x06001B6C RID: 7020 RVA: 0x0008A458 File Offset: 0x00088658
		public static int GetItemStackSpaceLeftFor(this IntVec3 c, Map map, ThingDef itemDef)
		{
			int num = 0;
			int num2 = 0;
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.category == ThingCategory.Item)
				{
					num2++;
				}
				if (list[i].def == itemDef)
				{
					num += Mathf.Max(list[i].def.stackLimit - list[i].stackCount, 0);
				}
			}
			return num + Mathf.Max(c.GetMaxItemsAllowedInCell(map) - num2, 0) * itemDef.stackLimit;
		}

		// Token: 0x06001B6D RID: 7021 RVA: 0x0008A4F0 File Offset: 0x000886F0
		public static bool IsBuildingInteractionCell(this IntVec3 cell, Map map)
		{
			for (int i = 0; i < 4; i++)
			{
				IntVec3 c = cell + GenAdj.CardinalDirections[i];
				if (c.InBounds(map))
				{
					Building edifice = c.GetEdifice(map);
					if (edifice != null && edifice.def.hasInteractionCell && edifice.InteractionCell == cell)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001B6E RID: 7022 RVA: 0x0008A550 File Offset: 0x00088750
		public static Building GetFirstBuilding(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				Building building = list[i] as Building;
				if (building != null)
				{
					return building;
				}
			}
			return null;
		}

		// Token: 0x06001B6F RID: 7023 RVA: 0x0008A590 File Offset: 0x00088790
		public static Pawn GetFirstPawn(this IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Pawn pawn = thingList[i] as Pawn;
				if (pawn != null)
				{
					return pawn;
				}
			}
			return null;
		}

		// Token: 0x06001B70 RID: 7024 RVA: 0x0008A5CC File Offset: 0x000887CC
		public static Mineable GetFirstMineable(this IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Mineable mineable = thingList[i] as Mineable;
				if (mineable != null)
				{
					return mineable;
				}
			}
			return null;
		}

		// Token: 0x06001B71 RID: 7025 RVA: 0x0008A608 File Offset: 0x00088808
		public static Blight GetFirstBlight(this IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Blight blight = thingList[i] as Blight;
				if (blight != null)
				{
					return blight;
				}
			}
			return null;
		}

		// Token: 0x06001B72 RID: 7026 RVA: 0x0008A644 File Offset: 0x00088844
		public static Skyfaller GetFirstSkyfaller(this IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Skyfaller skyfaller = thingList[i] as Skyfaller;
				if (skyfaller != null)
				{
					return skyfaller;
				}
			}
			return null;
		}

		// Token: 0x06001B73 RID: 7027 RVA: 0x0008A680 File Offset: 0x00088880
		public static IPlantToGrowSettable GetPlantToGrowSettable(this IntVec3 c, Map map)
		{
			IPlantToGrowSettable plantToGrowSettable = c.GetEdifice(map) as IPlantToGrowSettable;
			if (plantToGrowSettable == null)
			{
				plantToGrowSettable = (c.GetZone(map) as IPlantToGrowSettable);
			}
			return plantToGrowSettable;
		}

		// Token: 0x06001B74 RID: 7028 RVA: 0x0008A6AC File Offset: 0x000888AC
		public static Building GetTransmitter(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.EverTransmitsPower)
				{
					return (Building)list[i];
				}
			}
			return null;
		}

		// Token: 0x06001B75 RID: 7029 RVA: 0x0008A6F8 File Offset: 0x000888F8
		public static Building_Door GetDoor(this IntVec3 c, Map map)
		{
			Building_Door building_Door = c.GetEdifice(map) as Building_Door;
			if (building_Door != null)
			{
				return building_Door;
			}
			return null;
		}

		// Token: 0x06001B76 RID: 7030 RVA: 0x0008A718 File Offset: 0x00088918
		public static Building GetFence(this IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			if (edifice != null && edifice.def.IsFence)
			{
				return edifice;
			}
			return null;
		}

		// Token: 0x06001B77 RID: 7031 RVA: 0x0008A740 File Offset: 0x00088940
		public static Building GetEdifice(this IntVec3 c, Map map)
		{
			return map.edificeGrid[c];
		}

		// Token: 0x06001B78 RID: 7032 RVA: 0x0008A74E File Offset: 0x0008894E
		public static Building GetEdificeSafe(this IntVec3 c, Map map)
		{
			if (!c.InBounds(map))
			{
				return null;
			}
			return map.edificeGrid[c];
		}

		// Token: 0x06001B79 RID: 7033 RVA: 0x0008A767 File Offset: 0x00088967
		public static Thing GetCover(this IntVec3 c, Map map)
		{
			return map.coverGrid[c];
		}

		// Token: 0x06001B7A RID: 7034 RVA: 0x0008A778 File Offset: 0x00088978
		public static Gas GetGas(this IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def.category == ThingCategory.Gas)
				{
					return (Gas)thingList[i];
				}
			}
			return null;
		}

		// Token: 0x06001B7B RID: 7035 RVA: 0x0008A7C0 File Offset: 0x000889C0
		public static bool IsInPrisonCell(this IntVec3 c, Map map)
		{
			Room roomOrAdjacent = c.GetRoomOrAdjacent(map, RegionType.Set_All);
			if (roomOrAdjacent != null)
			{
				return roomOrAdjacent.IsPrisonCell;
			}
			string str = "Checking prison cell status of ";
			IntVec3 intVec = c;
			Log.Error(str + intVec.ToString() + " which is not in or adjacent to a room.");
			return false;
		}

		// Token: 0x06001B7C RID: 7036 RVA: 0x0008A808 File Offset: 0x00088A08
		public static bool UsesOutdoorTemperature(this IntVec3 c, Map map)
		{
			Room room = c.GetRoom(map);
			if (room != null)
			{
				return room.UsesOutdoorTemperature;
			}
			Building edifice = c.GetEdifice(map);
			if (edifice != null)
			{
				IntVec3[] array = GenAdj.CellsAdjacent8Way(edifice).ToArray<IntVec3>();
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].InBounds(map))
					{
						room = array[i].GetRoom(map);
						if (room != null && room.UsesOutdoorTemperature)
						{
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06001B7D RID: 7037 RVA: 0x0008A879 File Offset: 0x00088A79
		public static bool IsPolluted(this IntVec3 c, Map map)
		{
			return map.pollutionGrid.IsPolluted(c);
		}

		// Token: 0x06001B7E RID: 7038 RVA: 0x0008A887 File Offset: 0x00088A87
		public static bool CanPollute(this IntVec3 c, Map map)
		{
			return map.pollutionGrid.CanPollute(c);
		}

		// Token: 0x06001B7F RID: 7039 RVA: 0x0008A895 File Offset: 0x00088A95
		public static bool CanUnpollute(this IntVec3 c, Map map)
		{
			return map.pollutionGrid.CanUnpollute(c);
		}

		// Token: 0x06001B80 RID: 7040 RVA: 0x0008A8A3 File Offset: 0x00088AA3
		public static void Pollute(this IntVec3 c, Map map, bool silent = false)
		{
			map.pollutionGrid.SetPolluted(c, true, silent);
		}

		// Token: 0x06001B81 RID: 7041 RVA: 0x0008A8B4 File Offset: 0x00088AB4
		public static void Unpollute(this IntVec3 c, Map map)
		{
			map.pollutionGrid.SetPolluted(c, false, false);
			if (map.snowGrid.GetDepth(c) > 1E-45f)
			{
				map.snowGrid.MakeMeshDirty(c);
			}
			if (ModsConfig.OdysseyActive && map.sandGrid.GetDepth(c) > 1E-45f)
			{
				map.sandGrid.MakeMeshDirty(c);
			}
		}

		// Token: 0x06001B82 RID: 7042 RVA: 0x0008A914 File Offset: 0x00088B14
		public static float GetFertility(this IntVec3 c, Map map)
		{
			return map.fertilityGrid.FertilityAt(c);
		}

		// Token: 0x040017E8 RID: 6120
		private static readonly HashSet<Region> connected = new HashSet<Region>();

		// Token: 0x040017E9 RID: 6121
		private static readonly Queue<Region> queue = new Queue<Region>();
	}
}
