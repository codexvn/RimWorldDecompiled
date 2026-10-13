using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EE7 RID: 7911
	public class RoomContents_SecurityRoom_Stockpile : RoomContentsWorker
	{
		// Token: 0x0600AF3C RID: 44860 RVA: 0x0032018A File Offset: 0x0031E38A
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			base.FillRoom(map, room, faction, threatPoints);
			RoomContents_SecurityRoom_Stockpile.SpawnAncientMachine(map, room);
			RoomContents_SecurityRoom_Stockpile.SpawnDestroyedConsoles(map, room);
		}

		// Token: 0x0600AF3D RID: 44861 RVA: 0x003201A8 File Offset: 0x0031E3A8
		private static void SpawnAncientMachine(Map map, LayoutRoom room)
		{
			RoomGenUtility.FillWithPadding(ThingDefOf.AncientMachine, 1, room, map, null, null, null, 1, null, false, false, null, null);
		}

		// Token: 0x0600AF3E RID: 44862 RVA: 0x003201D4 File Offset: 0x0031E3D4
		private static void SpawnDestroyedConsoles(Map map, LayoutRoom room)
		{
			RoomContents_SecurityRoom_Stockpile.<>c__DisplayClass4_0 CS$<>8__locals1 = new RoomContents_SecurityRoom_Stockpile.<>c__DisplayClass4_0();
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.room = room;
			float num = (float)CS$<>8__locals1.room.rects.Sum((CellRect r) => r.ContractedBy(1).EdgeCellsCount) / 10f;
			int count = Mathf.Max(Mathf.RoundToInt(RoomContents_SecurityRoom_Stockpile.DestroyedConsolesPer10EdgeCells.RandomInRange * num), 1);
			RoomGenUtility.FillAroundEdges(ThingDefOf.AncientDestroyedConsole, count, RoomContents_SecurityRoom_Stockpile.DestroyedConsolesGroupSize, CS$<>8__locals1.room, CS$<>8__locals1.map, new Func<IntVec3, Rot4, CellRect, bool>(CS$<>8__locals1.<SpawnDestroyedConsoles>g__ConsoleValidator|1), null, 1, 0, null, true, RotationDirection.Opposite, null, null);
		}

		// Token: 0x04007855 RID: 30805
		private static readonly FloatRange DestroyedConsolesPer10EdgeCells = new FloatRange(1f, 3f);

		// Token: 0x04007856 RID: 30806
		private static readonly IntRange DestroyedConsolesGroupSize = new IntRange(2, 3);
	}
}
