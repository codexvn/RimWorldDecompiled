using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F00 RID: 7936
	public class RoomContents_ShelfedCorridor : RoomContents_Corridor
	{
		// Token: 0x17001AEA RID: 6890
		// (get) Token: 0x0600AFBC RID: 44988 RVA: 0x00321432 File Offset: 0x0031F632
		protected override ThingDef DoorThing
		{
			get
			{
				return ThingDefOf.AncientBlastDoor;
			}
		}

		// Token: 0x17001AEB RID: 6891
		// (get) Token: 0x0600AFBD RID: 44989 RVA: 0x0032173C File Offset: 0x0031F93C
		protected override IntRange ExteriorDoorCount
		{
			get
			{
				return new IntRange(2, 3);
			}
		}

		// Token: 0x0600AFBE RID: 44990 RVA: 0x00321C54 File Offset: 0x0031FE54
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			base.FillRoom(map, room, faction, threatPoints);
			RoomContents_ShelfedCorridor.SpawnShelves(map, room);
		}

		// Token: 0x0600AFBF RID: 44991 RVA: 0x00321C68 File Offset: 0x0031FE68
		private static void SpawnShelves(Map map, LayoutRoom room)
		{
			float num = (float)room.rects.Sum((CellRect r) => r.ContractedBy(1).EdgeCellsCount) / 10f;
			int count = Mathf.Max(Mathf.RoundToInt(RoomContents_ShelfedCorridor.ShelvesPer10EdgeCells.RandomInRange * num), 1);
			RoomGenUtility.FillAroundEdges(ThingDefOf.Shelf, count, RoomContents_ShelfedCorridor.ShelfGroupSizeRange, room, map, null, null, 1, 0, ThingDefOf.Steel, true, RotationDirection.Opposite, null, null);
		}

		// Token: 0x04007880 RID: 30848
		private static readonly FloatRange ShelvesPer10EdgeCells = new FloatRange(0.35f, 0.35f);

		// Token: 0x04007881 RID: 30849
		private static readonly IntRange ShelfGroupSizeRange = new IntRange(1, 2);
	}
}
