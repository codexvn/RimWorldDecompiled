using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EA5 RID: 7845
	public static class ComplexUtility
	{
		// Token: 0x0600AE0E RID: 44558 RVA: 0x0031A184 File Offset: 0x00318384
		public static bool TryFindRandomSpawnCell(ThingDef def, LayoutRoom room, Map map, out IntVec3 spawnPosition, int gap = 1, Rot4? rot = null)
		{
			ComplexUtility.<>c__DisplayClass0_0 CS$<>8__locals1 = new ComplexUtility.<>c__DisplayClass0_0();
			CS$<>8__locals1.rot = rot;
			CS$<>8__locals1.def = def;
			CS$<>8__locals1.gap = gap;
			CS$<>8__locals1.map = map;
			ThingDef def2 = CS$<>8__locals1.def;
			Map map2 = CS$<>8__locals1.map;
			Func<IntVec3, bool> validator = new Func<IntVec3, bool>(CS$<>8__locals1.<TryFindRandomSpawnCell>g__Validator|0);
			return room.TryGetRandomCellInRoom(def2, map2, out spawnPosition, null, 1, 0, validator, false);
		}

		// Token: 0x0600AE0F RID: 44559 RVA: 0x0031A1E4 File Offset: 0x003183E4
		public static string SpawnRoomEnteredTrigger(LayoutRoom room, Map map)
		{
			string text = "RoomEntered" + Find.UniqueIDsManager.GetNextSignalTagID().ToString();
			foreach (CellRect rect in room.rects)
			{
				RectTrigger rectTrigger = (RectTrigger)ThingMaker.MakeThing(ThingDefOf.RectTrigger, null);
				rectTrigger.signalTag = text;
				rectTrigger.Rect = rect;
				GenSpawn.Spawn(rectTrigger, rect.CenterCell, map, WipeMode.Vanish);
			}
			return text;
		}

		// Token: 0x0600AE10 RID: 44560 RVA: 0x0031A27C File Offset: 0x0031847C
		public static string SpawnRadialDistanceTrigger(IEnumerable<Thing> things, Map map, int radius)
		{
			string text = "RandomTrigger" + Find.UniqueIDsManager.GetNextSignalTagID().ToString();
			foreach (Thing thing in things)
			{
				RadialTrigger radialTrigger = (RadialTrigger)ThingMaker.MakeThing(ThingDefOf.RadialTrigger, null);
				radialTrigger.signalTag = text;
				radialTrigger.maxRadius = radius;
				radialTrigger.lineOfSight = true;
				GenSpawn.Spawn(radialTrigger, thing.Position, map, WipeMode.Vanish);
			}
			return text;
		}
	}
}
