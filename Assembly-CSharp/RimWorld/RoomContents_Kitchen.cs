using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EE0 RID: 7904
	public class RoomContents_Kitchen : RoomContentsWorker
	{
		// Token: 0x17001AC3 RID: 6851
		// (get) Token: 0x0600AF13 RID: 44819 RVA: 0x0031F4FC File Offset: 0x0031D6FC
		protected virtual FloatRange GroupsPer100Cells
		{
			get
			{
				return new FloatRange(1f, 2f);
			}
		}

		// Token: 0x17001AC4 RID: 6852
		// (get) Token: 0x0600AF14 RID: 44820 RVA: 0x0031F50D File Offset: 0x0031D70D
		protected virtual IntRange ItemsPerGroup
		{
			get
			{
				return new IntRange(1, 3);
			}
		}

		// Token: 0x17001AC5 RID: 6853
		// (get) Token: 0x0600AF15 RID: 44821 RVA: 0x0031F516 File Offset: 0x0031D716
		protected virtual IntRange SinksCount
		{
			get
			{
				return new IntRange(3, 6);
			}
		}

		// Token: 0x17001AC6 RID: 6854
		// (get) Token: 0x0600AF16 RID: 44822 RVA: 0x0031F51F File Offset: 0x0031D71F
		protected virtual IntRange SinkMaxRange
		{
			get
			{
				return new IntRange(2, 4);
			}
		}

		// Token: 0x0600AF17 RID: 44823 RVA: 0x0031F528 File Offset: 0x0031D728
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			this.SpawnEdgeThings(ThingDefOf.AncientMicrowave, map, room);
			this.SpawnEdgeThings(ThingDefOf.AncientRefrigerator, map, room);
			this.SpawnEdgeThings(ThingDefOf.AncientStove, map, room);
			this.SpawnSinks(map, room);
			this.ScatterTables(map, room);
			foreach (IntVec3 c in room.Cells)
			{
				if (Rand.Chance(0.75f))
				{
					map.terrainGrid.SetTerrain(c, TerrainDefOf.AncientTile);
				}
			}
			base.FillRoom(map, room, faction, threatPoints);
		}

		// Token: 0x0600AF18 RID: 44824 RVA: 0x0031F5D0 File Offset: 0x0031D7D0
		private void ScatterTables(Map map, LayoutRoom room)
		{
			Rot4 value = Rot4.North;
			CellRect boundary = room.Boundary;
			if (boundary.Height > boundary.Width)
			{
				value = Rot4.East;
			}
			RoomGenUtility.FillWithPadding(ThingDefOf.Table1x2c, this.GetCount(room), room, map, new Rot4?(value), null, null, 1, ThingDefOf.Steel, false, false, null, null);
		}

		// Token: 0x0600AF19 RID: 44825 RVA: 0x0031F628 File Offset: 0x0031D828
		private void SpawnSinks(Map map, LayoutRoom room)
		{
			IntVec3 centerCell;
			if (!room.TryGetRandomCellInRoom(map, out centerCell, 4, 0, null, false))
			{
				centerCell = room.rects[0].CenterCell;
			}
			GenSpawn.SpawnIrregularLump(ThingDefOf.AncientKitchenSink, centerCell, map, this.SinksCount, this.SinkMaxRange, WipeMode.Vanish, (IntVec3 c) => this.IsValid(c, map, room), null, null, null, null);
		}

		// Token: 0x0600AF1A RID: 44826 RVA: 0x0031F6B4 File Offset: 0x0031D8B4
		private void SpawnEdgeThings(ThingDef def, Map map, LayoutRoom room)
		{
			RoomGenUtility.FillAroundEdges(def, this.GetCount(room), this.ItemsPerGroup, room, map, null, null, 1, 0, null, true, RotationDirection.Opposite, null, null);
		}

		// Token: 0x0600AF1B RID: 44827 RVA: 0x0031F6E0 File Offset: 0x0031D8E0
		private int GetCount(LayoutRoom room)
		{
			float num = (float)room.Area / 100f;
			return Mathf.Max(Mathf.RoundToInt(this.GroupsPer100Cells.RandomInRange * num), 1);
		}

		// Token: 0x0600AF1C RID: 44828 RVA: 0x0031F716 File Offset: 0x0031D916
		private bool IsValid(IntVec3 cell, Map map, LayoutRoom room)
		{
			return room.Contains(cell, 1) && cell.GetEdifice(map) == null;
		}

		// Token: 0x04007846 RID: 30790
		private const float FloorSteelPercentage = 0.75f;
	}
}
