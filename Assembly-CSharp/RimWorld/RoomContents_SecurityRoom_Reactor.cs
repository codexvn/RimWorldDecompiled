using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F1A RID: 7962
	public class RoomContents_SecurityRoom_Reactor : RoomContentsWorker
	{
		// Token: 0x0600B015 RID: 45077 RVA: 0x00322C78 File Offset: 0x00320E78
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			float num = (float)room.rects.Sum((CellRect r) => r.ContractedBy(1).EdgeCellsCount) / 10f;
			int count = Mathf.Max(Mathf.RoundToInt(RoomContents_SecurityRoom_Reactor.StandardRechargersPer10EdgeCells.RandomInRange * num), 1);
			RoomGenUtility.FillAroundEdges(ThingDefOf.AncientStandardRecharger, count, RoomContents_SecurityRoom_Reactor.StandardRechargersGroupSize, room, map, null, null, 1, 0, null, true, RotationDirection.Opposite, null, null);
			count = Mathf.Max(Mathf.RoundToInt(RoomContents_SecurityRoom_Reactor.BasicRechargersPer10EdgeCells.RandomInRange * num), 1);
			RoomGenUtility.FillAroundEdges(ThingDefOf.AncientBasicRecharger, count, RoomContents_SecurityRoom_Reactor.BasicRechargersGroupSize, room, map, null, null, 1, 0, null, true, RotationDirection.Opposite, null, null);
			RoomContents_SecurityRoom_Reactor.SpawnSecurityConsole(map, room);
			base.FillRoom(map, room, faction, threatPoints);
		}

		// Token: 0x0600B016 RID: 45078 RVA: 0x00322D38 File Offset: 0x00320F38
		private static void SpawnSecurityConsole(Map map, LayoutRoom room)
		{
			RoomContents_SecurityRoom_Reactor.<>c__DisplayClass5_0 CS$<>8__locals1 = new RoomContents_SecurityRoom_Reactor.<>c__DisplayClass5_0();
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.room = room;
			RoomGenUtility.FillAroundEdges(ThingDefOf.AncientSecurityTerminal, 1, IntRange.One, CS$<>8__locals1.room, CS$<>8__locals1.map, new Func<IntVec3, Rot4, CellRect, bool>(CS$<>8__locals1.<SpawnSecurityConsole>g__ConsoleValidator|0), null, 1, 0, null, true, RotationDirection.Opposite, null, null);
		}

		// Token: 0x040078AB RID: 30891
		private static readonly FloatRange StandardRechargersPer10EdgeCells = new FloatRange(0.2f, 1f);

		// Token: 0x040078AC RID: 30892
		private static readonly IntRange StandardRechargersGroupSize = new IntRange(1, 1);

		// Token: 0x040078AD RID: 30893
		private static readonly FloatRange BasicRechargersPer10EdgeCells = new FloatRange(0.5f, 1.5f);

		// Token: 0x040078AE RID: 30894
		private static readonly IntRange BasicRechargersGroupSize = new IntRange(1, 2);
	}
}
