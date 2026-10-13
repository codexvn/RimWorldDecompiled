using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F17 RID: 7959
	public class RoomContents_Reactor : RoomContentsWorker
	{
		// Token: 0x0600B009 RID: 45065 RVA: 0x003229E8 File Offset: 0x00320BE8
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			CellRect cellRect = (from r in room.rects
			where r.Width >= 6 && r.Height >= 6
			orderby r.Area descending
			select r).FirstOrDefault<CellRect>();
			if (cellRect == default(CellRect))
			{
				Log.Error("Failed to place generator.");
				return;
			}
			RoomContents_Reactor.SpawnReactor(map, cellRect, faction);
			base.FillRoom(map, room, faction, threatPoints);
			RoomContents_Reactor.SpawnDestroyedConsoles(map, room);
		}

		// Token: 0x0600B00A RID: 45066 RVA: 0x00322A80 File Offset: 0x00320C80
		private static void SpawnDestroyedConsoles(Map map, LayoutRoom room)
		{
			RoomContents_Reactor.<>c__DisplayClass3_0 CS$<>8__locals1 = new RoomContents_Reactor.<>c__DisplayClass3_0();
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.room = room;
			float num = (float)CS$<>8__locals1.room.rects.Sum((CellRect r) => r.ContractedBy(1).EdgeCellsCount) / 10f;
			int count = Mathf.Max(Mathf.RoundToInt(RoomContents_Reactor.DestroyedConsolesPer10EdgeCells.RandomInRange * num), 1);
			RoomGenUtility.FillAroundEdges(ThingDefOf.AncientDestroyedConsole, count, RoomContents_Reactor.DestroyedConsolesGroupSize, CS$<>8__locals1.room, CS$<>8__locals1.map, new Func<IntVec3, Rot4, CellRect, bool>(CS$<>8__locals1.<SpawnDestroyedConsoles>g__ConsoleValidator|1), null, 1, 0, null, true, RotationDirection.Opposite, null, null);
		}

		// Token: 0x0600B00B RID: 45067 RVA: 0x00322B24 File Offset: 0x00320D24
		private static void SpawnReactor(Map map, CellRect largest, Faction faction)
		{
			IntVec3 zero = IntVec3.Zero;
			if (largest.Width % 2 == 0)
			{
				zero.x = 1;
			}
			if (largest.Height % 2 == 0)
			{
				zero.z = 1;
			}
			Thing thing = ThingMaker.MakeThing(ThingDefOf.AncientGravReactor, null);
			thing.SetFaction(faction ?? Faction.OfAncientsHostile, null);
			GenSpawn.Spawn(thing, largest.CenterCell - zero, map, Rot4.Random, WipeMode.Vanish, false, false);
		}

		// Token: 0x040078A3 RID: 30883
		private static readonly FloatRange DestroyedConsolesPer10EdgeCells = new FloatRange(1f, 3f);

		// Token: 0x040078A4 RID: 30884
		private static readonly IntRange DestroyedConsolesGroupSize = new IntRange(2, 3);
	}
}
