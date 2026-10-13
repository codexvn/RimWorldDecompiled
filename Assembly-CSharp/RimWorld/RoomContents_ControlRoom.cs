using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001ECE RID: 7886
	public class RoomContents_ControlRoom : RoomContentsWorker
	{
		// Token: 0x0600AED0 RID: 44752 RVA: 0x0031E310 File Offset: 0x0031C510
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			base.FillRoom(map, room, faction, threatPoints);
			RoomGenUtility.FillAroundEdges(ThingDefOf.AncientSecurityTerminal, 1, IntRange.One, room, map, null, null, 1, 0, null, true, RotationDirection.Opposite, null, null);
			RoomContents_ControlRoom.SpawnAncientMachines(map, room);
			RoomContents_ControlRoom.SpawnDestroyedConsoles(map, room);
		}

		// Token: 0x0600AED1 RID: 44753 RVA: 0x0031E354 File Offset: 0x0031C554
		private static void SpawnAncientMachines(Map map, LayoutRoom room)
		{
			float num = (float)room.Area / 100f;
			int count = Mathf.Max(Mathf.RoundToInt(RoomContents_ControlRoom.AncientMachinesPer100Cells.RandomInRange * num), 1);
			RoomGenUtility.FillWithPadding(ThingDefOf.AncientMachine, count, room, map, null, null, null, 1, null, false, false, null, null);
		}

		// Token: 0x0600AED2 RID: 44754 RVA: 0x0031E3A8 File Offset: 0x0031C5A8
		private static void SpawnDestroyedConsoles(Map map, LayoutRoom room)
		{
			RoomContents_ControlRoom.<>c__DisplayClass5_0 CS$<>8__locals1 = new RoomContents_ControlRoom.<>c__DisplayClass5_0();
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.room = room;
			float num = (float)CS$<>8__locals1.room.rects.Sum((CellRect r) => r.ContractedBy(1).EdgeCellsCount) / 10f;
			int count = Mathf.Max(Mathf.RoundToInt(RoomContents_ControlRoom.DestroyedConsolesPer10EdgeCells.RandomInRange * num), 1);
			RoomGenUtility.FillAroundEdges(ThingDefOf.AncientDestroyedConsole, count, RoomContents_ControlRoom.DestroyedConsolesGroupSize, CS$<>8__locals1.room, CS$<>8__locals1.map, new Func<IntVec3, Rot4, CellRect, bool>(CS$<>8__locals1.<SpawnDestroyedConsoles>g__ConsoleValidator|1), null, 1, 0, null, true, RotationDirection.Opposite, null, null);
		}

		// Token: 0x04007826 RID: 30758
		private static readonly FloatRange AncientMachinesPer100Cells = new FloatRange(3f, 5f);

		// Token: 0x04007827 RID: 30759
		private static readonly FloatRange DestroyedConsolesPer10EdgeCells = new FloatRange(1f, 3f);

		// Token: 0x04007828 RID: 30760
		private static readonly IntRange DestroyedConsolesGroupSize = new IntRange(2, 3);
	}
}
