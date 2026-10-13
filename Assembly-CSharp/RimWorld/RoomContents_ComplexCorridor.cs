using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EFC RID: 7932
	public class RoomContents_ComplexCorridor : RoomContents_Corridor
	{
		// Token: 0x17001AE5 RID: 6885
		// (get) Token: 0x0600AFA7 RID: 44967 RVA: 0x0032173C File Offset: 0x0031F93C
		protected override IntRange ExteriorDoorCount
		{
			get
			{
				return new IntRange(2, 3);
			}
		}

		// Token: 0x0600AFA8 RID: 44968 RVA: 0x00321745 File Offset: 0x0031F945
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			base.FillRoom(map, room, faction, threatPoints);
			RoomContents_ComplexCorridor.SpawnShelves(map, room);
		}

		// Token: 0x0600AFA9 RID: 44969 RVA: 0x0032175C File Offset: 0x0031F95C
		private static void SpawnShelves(Map map, LayoutRoom room)
		{
			float num = (float)room.rects.Sum((CellRect r) => r.ContractedBy(1).EdgeCellsCount) / 10f;
			int count = Mathf.Max(Mathf.RoundToInt(RoomContents_ComplexCorridor.ShelvesPer10EdgeCells.RandomInRange * num), 1);
			RoomGenUtility.FillAroundEdges(ThingDefOf.Shelf, count, RoomContents_ComplexCorridor.ShelfGroupSizeRange, room, map, null, null, 1, 0, ThingDefOf.Steel, true, RotationDirection.Opposite, null, null);
		}

		// Token: 0x0400787C RID: 30844
		private static readonly FloatRange ShelvesPer10EdgeCells = new FloatRange(0.35f, 0.35f);

		// Token: 0x0400787D RID: 30845
		private static readonly IntRange ShelfGroupSizeRange = new IntRange(1, 2);
	}
}
