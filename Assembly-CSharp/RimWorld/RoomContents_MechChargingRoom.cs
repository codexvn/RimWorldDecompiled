using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EE3 RID: 7907
	public class RoomContents_MechChargingRoom : RoomContentsWorker
	{
		// Token: 0x0600AF22 RID: 44834 RVA: 0x0031F858 File Offset: 0x0031DA58
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			base.FillRoom(map, room, faction, threatPoints);
			RoomContents_MechChargingRoom.SpawnMechRechargers(map, room);
		}

		// Token: 0x0600AF23 RID: 44835 RVA: 0x0031F86C File Offset: 0x0031DA6C
		private static void SpawnMechRechargers(Map map, LayoutRoom room)
		{
			float num = (float)room.rects.Sum((CellRect r) => r.ContractedBy(1).EdgeCellsCount) / 10f;
			int count = Mathf.Max(Mathf.RoundToInt(RoomContents_MechChargingRoom.StandardRechargersPer10EdgeCells.RandomInRange * num), 1);
			RoomGenUtility.FillAroundEdges(ThingDefOf.AncientStandardRecharger, count, RoomContents_MechChargingRoom.StandardRechargersGroupSize, room, map, null, null, 1, 0, null, true, RotationDirection.Opposite, null, null);
			count = Mathf.Max(Mathf.RoundToInt(RoomContents_MechChargingRoom.BasicRechargersPer10EdgeCells.RandomInRange * num), 1);
			RoomGenUtility.FillAroundEdges(ThingDefOf.AncientBasicRecharger, count, RoomContents_MechChargingRoom.BasicRechargersGroupSize, room, map, null, null, 1, 0, null, true, RotationDirection.Opposite, null, null);
		}

		// Token: 0x0400784B RID: 30795
		private static readonly FloatRange StandardRechargersPer10EdgeCells = new FloatRange(0.2f, 1f);

		// Token: 0x0400784C RID: 30796
		private static readonly IntRange StandardRechargersGroupSize = new IntRange(1, 1);

		// Token: 0x0400784D RID: 30797
		private static readonly FloatRange BasicRechargersPer10EdgeCells = new FloatRange(0.5f, 1.5f);

		// Token: 0x0400784E RID: 30798
		private static readonly IntRange BasicRechargersGroupSize = new IntRange(1, 2);
	}
}
