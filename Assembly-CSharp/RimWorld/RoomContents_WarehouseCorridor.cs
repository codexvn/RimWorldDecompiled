using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F02 RID: 7938
	public class RoomContents_WarehouseCorridor : RoomContents_Corridor
	{
		// Token: 0x17001AEC RID: 6892
		// (get) Token: 0x0600AFC5 RID: 44997 RVA: 0x0032173C File Offset: 0x0031F93C
		protected override IntRange ExteriorDoorCount
		{
			get
			{
				return new IntRange(2, 3);
			}
		}

		// Token: 0x0600AFC6 RID: 44998 RVA: 0x00321D2D File Offset: 0x0031FF2D
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			base.FillRoom(map, room, faction, threatPoints);
			RoomContents_WarehouseCorridor.SpawnShelves(map, room);
		}

		// Token: 0x0600AFC7 RID: 44999 RVA: 0x00321D44 File Offset: 0x0031FF44
		private static void SpawnShelves(Map map, LayoutRoom room)
		{
			float num = (float)room.rects.Sum((CellRect r) => r.ContractedBy(1).EdgeCellsCount) / 10f;
			int count = Mathf.Max(Mathf.RoundToInt(RoomContents_WarehouseCorridor.ShelvesPer10EdgeCells.RandomInRange * num), 1);
			RoomGenUtility.FillAroundEdges(ThingDefOf.Shelf, count, RoomContents_WarehouseCorridor.ShelfGroupSizeRange, room, map, null, null, 1, 0, ThingDefOf.Steel, true, RotationDirection.Opposite, null, null);
		}

		// Token: 0x04007884 RID: 30852
		private static readonly FloatRange ShelvesPer10EdgeCells = new FloatRange(0.7f, 1.4f);

		// Token: 0x04007885 RID: 30853
		private static readonly IntRange ShelfGroupSizeRange = new IntRange(1, 2);
	}
}
