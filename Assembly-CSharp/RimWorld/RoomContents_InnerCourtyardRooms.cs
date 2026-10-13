using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EDF RID: 7903
	public class RoomContents_InnerCourtyardRooms : RoomContentsWorker
	{
		// Token: 0x0600AF0B RID: 44811 RVA: 0x0031F0FC File Offset: 0x0031D2FC
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			this.placedCrates = 0;
			CellRect rect = room.rects[0];
			foreach (IntVec3 cell in rect.ContractedBy(2).EdgeCells)
			{
				if (!RoomContents_InnerCourtyardRooms.ContainedWithinOtherRect(rect, cell, room, 1))
				{
					RoomContents_InnerCourtyardRooms.SpawnWall(cell, map);
				}
			}
			this.SpawnInteriorRooms(map, rect, room);
			base.FillRoom(map, room, faction, threatPoints);
		}

		// Token: 0x0600AF0C RID: 44812 RVA: 0x0031F188 File Offset: 0x0031D388
		private void SpawnInteriorRooms(Map map, CellRect rect, LayoutRoom room)
		{
			CellRect cellRect = rect.ContractedBy(3);
			Queue<CellRect> queue = RoomGenUtility.SubdividedIntoRooms_NewTemp(map, cellRect, 4, 1, 2, ThingDefOf.AncientFortifiedWall, null);
			CellRect rect2;
			while (queue.TryDequeue(out rect2))
			{
				this.DoSubroomRoomInterior(map, rect2, cellRect, room);
			}
		}

		// Token: 0x0600AF0D RID: 44813 RVA: 0x0031F1C8 File Offset: 0x0031D3C8
		private void DoSubroomRoomInterior(Map map, CellRect rect, CellRect container, LayoutRoom room)
		{
			foreach (IntVec3 c in rect.Cells)
			{
				map.terrainGrid.SetTerrain(c, TerrainDefOf.AncientTile);
			}
			float num = (float)rect.Area / 10f;
			int num2 = 0;
			int num3 = Mathf.Max(Mathf.RoundToInt(1f * num), 1);
			foreach (IntVec3 intVec in rect.ContractedBy(1).Cells.InRandomOrder(null))
			{
				if (RoomContents_InnerCourtyardRooms.IsValidCrateCell(intVec, map))
				{
					this.SpawnBox(intVec, map);
					if (++num2 >= num3)
					{
						break;
					}
				}
			}
			CellRect item = rect.ExpandedBy(1);
			ThingDef def = ModsConfig.OdysseyActive ? ThingDefOf.AncientBlastDoor : ThingDefOf.Door;
			ThingDef stuff = ModsConfig.OdysseyActive ? null : ThingDefOf.Steel;
			foreach (IntVec3 intVec2 in item.EdgeCells.InRandomOrder(null))
			{
				if (RoomGenUtility.IsGoodForDoor(intVec2, map) && !container.ContractedBy(1).Contains(intVec2))
				{
					GenSpawn.Spawn(ThingMaker.MakeThing(def, stuff), intVec2, map, WipeMode.Vanish);
					break;
				}
			}
			RoomGenUtility.SpawnWallAttatchments(room.sketch.layoutSketch.WallLampThing, map, new List<CellRect>(1)
			{
				item
			}, IntRange.Between(1, 3), null, null, null);
		}

		// Token: 0x0600AF0E RID: 44814 RVA: 0x0031F388 File Offset: 0x0031D588
		private void SpawnBox(IntVec3 cell, Map map)
		{
			ThingSetMakerDef mapGen_HighValueCrate = ThingSetMakerDefOf.MapGen_HighValueCrate;
			int num = this.placedCrates;
			this.placedCrates = num + 1;
			RoomGenUtility.SpawnHermeticCrate(cell, map, mapGen_HighValueCrate, num < 1, null);
		}

		// Token: 0x0600AF0F RID: 44815 RVA: 0x0031F3B8 File Offset: 0x0031D5B8
		private static bool IsValidCrateCell(IntVec3 pos, Map map)
		{
			foreach (IntVec3 intVec in pos.RectAbout(ThingDefOf.AncientHermeticCrate.Size, Rot4.South).Cells)
			{
				if (intVec.GetEdifice(map) != null)
				{
					return false;
				}
				using (List<IntVec3>.Enumerator enumerator2 = GenAdjFast.AdjacentCells8Way(intVec).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.GetEdifice(map) != null)
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x0600AF10 RID: 44816 RVA: 0x0031F46C File Offset: 0x0031D66C
		private static void SpawnWall(IntVec3 cell, Map map)
		{
			GenSpawn.Spawn(ThingDefOf.AncientFortifiedWall, cell, map, WipeMode.Vanish);
		}

		// Token: 0x0600AF11 RID: 44817 RVA: 0x0031F47C File Offset: 0x0031D67C
		private static bool ContainedWithinOtherRect(CellRect rect, IntVec3 cell, LayoutRoom room, int contractedBy)
		{
			if (!rect.EdgeCells.Contains(cell))
			{
				return false;
			}
			foreach (CellRect rhs in room.rects)
			{
				if (!(rect == rhs) && rhs.ContractedBy(contractedBy).Contains(cell))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04007842 RID: 30786
		private const int NormalCrates = 1;

		// Token: 0x04007843 RID: 30787
		private const int CratesPer10Cells = 1;

		// Token: 0x04007844 RID: 30788
		private const int InnerWallContract = 3;

		// Token: 0x04007845 RID: 30789
		private int placedCrates;
	}
}
